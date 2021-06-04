using HomeDoctor.Business.IService;
using HomeDoctor.Business.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.ViewModel.RequestModel;

namespace HomeDoctor.Business.Quartz.Jobs
{
    public class Action00AMJob : IJob
    {
        private readonly IServiceProvider _provider;

        public Action00AMJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var currentTime = DateTime.Now;
            Console.WriteLine("Excute 00AM : "+ currentTime);
            if (currentTime.Hour == 0)
            {
                {
                    using (var scope = _provider.CreateScope())
                    {                        
                        var _serContract = scope.ServiceProvider.GetService<IContractService>();
                        var _serVitalSign = scope.ServiceProvider.GetService<IVitalSignService>();
                        var _serNoti = scope.ServiceProvider.GetService<INotificationService>();
                        var _serFB = scope.ServiceProvider.GetService<IFirebaseFCMService>();
                        var _uow = scope.ServiceProvider.GetService<IUnitOfWork>();
                        //  Contract
                        await ActiveContract(_serContract, _serNoti, _serFB, _uow);
                        await LockContract(_serContract, _serFB, _uow);                       
                        await FinishContract(_serContract, _serNoti, _serFB, _uow);
                        await FinishPrescription(_uow);
                        //await InsertVitalSignValueEveryDay(_uow, _serVitalSign);
                        // After 3 day not approved or signed
                        await CancelContract(_serContract, _uow);                
                    }
                }
            }
        }
        private async Task LockContract(IContractService _serContract,IFirebaseFCMService _serFirebase,IUnitOfWork _uow)
        {
            Console.WriteLine("LockContract : " + DateTime.Now);
            var contracts = await _serContract.GetAllContractsByStatus("ACTIVE");
            if (contracts != null)
            {
                var tmp = contracts.Where(x => x.HealthRecord.AppointmentFirst == false && 
                x.HealthRecord.VitalSignScheduleFirst == false).ToList();
                if (tmp.Any()){
                    var currentTime = DateTime.Now;
                    var tmp2 = tmp.Where(x => x.DateStarted.Date.AddDays(4) <= currentTime.Date)
                        .Select(x =>
                        {
                            x.Status = "LOCKED";
                            x.DateLocked = currentTime;
                            x.ReasonLocked = "Sau 4 ngày không ra bất kỳ đơn thuốc hoặc cuộc hẹn nào cho bệnh nhân";
                            return x;
                        })
                        .ToList();
                    if (tmp2.Any())
                    {
                        var _repoContract = _uow.GetRepository<Contract>();
                        var tmpList = new List<Notification>();
                        if (await _repoContract.UpdateRange(tmp2))
                        {
                            await _uow.CommitAsync();
                            foreach (var x in tmp2)
                            {
                                // save notification for Doctor 
                                var notiD = new Notification()
                                {
                                    AccountId = x.Doctor.AccountId,
                                    AccountSendId = x.Patient.AccountId,
                                    ContractId = x.ContractId,
                                    OnSystem = true,
                                    NotificationTypeId = 16,
                                    DateCreate = DateTime.Now
                                };
                                tmpList.Add(notiD);
                                var notiP = new Notification()
                                {
                                    AccountId = x.Patient.AccountId,
                                    AccountSendId = x.Doctor.AccountId,
                                    ContractId = x.ContractId,
                                    OnSystem = true,
                                    NotificationTypeId = 21,
                                    DateCreate = DateTime.Now,
                                };
                                tmpList.Add(notiP);
                            }                                                                             
                            // Gop 2 list                                                       
                            var _repoNoti = _uow.GetRepository<Notification>();
                            if(await _repoNoti.InsertRange(tmpList))
                            {
                                await _uow.CommitAsync();
                            }                           
                            Console.WriteLine("Lock " + tmp2.Count + "contract");
                            //push noti Firebase for docotor
                            foreach (var noti in tmpList)
                            {
                                if(noti.NotificationTypeId == 16)
                                {
                                    _serFirebase.PushNotification(2, noti.AccountSendId.GetValueOrDefault(), noti.AccountId, 16, noti.ContractId, null, null, null);
                                }
                                else
                                {
                                    _serFirebase.PushNotification(1, noti.AccountSendId.GetValueOrDefault(), noti.AccountId, 21, noti.ContractId, null, null, null);
                                }
                                
                            }                          
                        }                      
                    }

                }
            }
        }
        private async Task InsertVitalSignValueEveryDay(IUnitOfWork _uow, IVitalSignService _serVitalSign)
        {
            var _repoMIVSSchedule = _uow.GetRepository<MedicalInstruction>();

            var miVSschedule = await _repoMIVSSchedule.GetDbSet().Where(x => x.HealthRecord.Contract.Status.Equals("ACTIVE") && x.MedicalInstructionTypeId == 8 && x.VitalSignSchedule.Status.Equals("ACTIVE")).Include(x => x.VitalSignSchedule).ToListAsync();
            if (miVSschedule.Any())
            {
                var currentTime = DateTime.Now;
                foreach (var n in miVSschedule)
                {                   
                    // Checked when Doctor create vitalSignSchedule inserted VitalSignvalue

                    if(n.VitalSignSchedule.DateStarted.Date < currentTime.Date)
                    {                    
                            //await _serVitalSign.InsertVitalSignEveryDay(n.VitalSignSchedule.VitalSignScheduleId);
                    }                   
                }
            }
        }

        private async Task ActiveContract(IContractService _serContract,INotificationService _serNoti,IFirebaseFCMService _serFB, IUnitOfWork _uow)
        {
            Console.WriteLine("ActiveContract "+ DateTime.Now);
            var contracts = await _serContract.GetAllContractsByStatus("SIGNED");
            if(contracts != null)
            {
                var currentTime = DateTime.Now;
                
                var tmp = contracts.Where(x => x.DateStarted.Date == currentTime.Date).Select(x =>
                {
                    Console.WriteLine("ContractId:" + x.ContractId);
                    x.Status = "ACTIVE";
                    return x;
                }).ToList();
                if (tmp.Any())
                {
                    var _repo = _uow.GetRepository<Contract>();

                    if (await _repo.UpdateRange(tmp))
                    {
                        await _uow.CommitAsync();
                        Console.WriteLine("Active " + tmp.Count + "contract");
                        foreach(var n in contracts)
                        {
                            // save notification of patient and doctor                          
                            var notiDoctor = new NotificationRequest()
                            {
                                AccountId = n.Doctor.AccountId,
                                AccountSendId = n.Patient.AccountId,
                                ContractId = n.ContractId,
                                OnSystem = true,
                                NotificationTypeId = 15,
                                
                            };
                            var notiPatient = new NotificationRequest()
                            {
                                AccountId = n.Patient.AccountId,
                                AccountSendId = n.Doctor.AccountId,
                                ContractId = n.ContractId,
                                OnSystem = true,
                                NotificationTypeId = 20,
                            };
                            await _serNoti.InsertNotification(notiDoctor);
                            await _serNoti.InsertNotification(notiPatient);
                            // push notification firebase for doctor
                            await _serFB.PushNotification(2, notiDoctor.AccountSendId.GetValueOrDefault(), notiDoctor.AccountId, 15, notiDoctor.ContractId, null, null,null);
                            // push notification for patient
                            await _serFB.PushNotification(1, notiPatient.AccountSendId.GetValueOrDefault(), notiPatient.AccountId, 20, notiPatient.ContractId, null, null,null);
                        }
                        await _uow.CommitAsync();                       
                    }
                }
            }
        }
        private async Task FinishContract(IContractService _serContract,INotificationService _serNoti, IFirebaseFCMService _serFB, IUnitOfWork _uow)
        {
            Console.WriteLine("FinishContract "+ DateTime.Now);

            var currentTime = DateTime.Now;
            var repoContract = _uow.GetRepository<Contract>();
            var contracts = await repoContract.GetDbSet().Include(x => x.Doctor).Include(x => x.Patient).Where(x => x.Status.Equals("ACTIVE") && x.DateFinished.AddDays(1).Date <= currentTime.Date).ToListAsync();
            if (contracts.Any())
            {
                contracts.Select(x => {
                    x.Status = "FINISHED";
                    return x;
                }).ToList();
                if (await repoContract.UpdateRange(contracts))
                {                   
                    Console.WriteLine("Finished :" + contracts.Count + " contract");
                    foreach(var n in contracts)
                    {
                        // cancel all medicalInstruction of contract
                        var _repoMI = _uow.GetRepository<MedicalInstruction>();
                        var mis = await _repoMI.GetDbSet().Include(x => x.VitalSignSchedule).Include(x => x.Prescription).Where(x => x.HealthRecord.ContractId == n.ContractId && (x.VitalSignSchedule != null || x.Prescription != null) && x.Status.Equals("DOCTOR") && x.MIShareFromId == null).ToListAsync();
                        if (mis.Any())
                        {
                            mis = mis.Where(x => (x.VitalSignSchedule != null ? x.VitalSignSchedule.Status.Equals("ACTIVE") : true) && (x.Prescription != null ? x.Prescription.Status.Equals("ACTIVE") : true)).ToList();
                            if (mis.Any())
                            {
                                foreach (var m in mis)
                                {
                                    if (m.MedicalInstructionTypeId == 8)
                                    {
                                        m.VitalSignSchedule.Status = "CANCEL";
                                    } else
                                    if (m.MedicalInstructionTypeId == 1)
                                    {
                                        m.Prescription.Status = "CANCEL";
                                    }
                                }
                            }                           
                        }
                        // save notification of patient and doctor                          
                        var notiDoctor = new NotificationRequest()
                        {
                            AccountId = n.Doctor.AccountId,
                            AccountSendId = n.Patient.AccountId,
                            ContractId = n.ContractId,
                            OnSystem = true,
                            NotificationTypeId = 19,
                        };
                        var notiPatient = new NotificationRequest()
                        {
                            AccountId = n.Patient.AccountId,
                            AccountSendId = n.Doctor.AccountId,
                            ContractId = n.ContractId,
                            OnSystem = true,
                            NotificationTypeId = 18,
                        };
                        await _serNoti.InsertNotification(notiDoctor);
                        await _serNoti.InsertNotification(notiPatient);
                        // push notification firebase for doctor
                        await _serFB.PushNotification(2, notiDoctor.AccountSendId.GetValueOrDefault(), notiDoctor.AccountId, 19, notiDoctor.ContractId, null, null,null);
                        // push notification for patient
                        await _serFB.PushNotification(1, notiPatient.AccountSendId.GetValueOrDefault(), notiPatient.AccountId, 18, notiPatient.ContractId, null, null,null);
                    }
                    await _uow.CommitAsync();                   
                }
            }
        }
        private async Task FinishPrescription(IUnitOfWork _uow)
        {
            Console.WriteLine("FinishPrescription " + DateTime.Now);
            var currentTime = DateTime.Now;
            var _repoPrescription = _uow.GetRepository<Prescription>();
            var psFinishToday = await _repoPrescription.GetDbSet().Where(x => x.DateFinished.AddDays(1).Date == currentTime.Date && x.DateStarted < currentTime).ToListAsync();
            if (psFinishToday.Any())
            {
                psFinishToday.Select(x =>
                {
                    x.Status = "FINISHED";
                    return x;
                });
                if (await _repoPrescription.UpdateRange(psFinishToday))
                {
                    await _uow.CommitAsync();
                }
            }
        }
        private async Task CancelContract(IContractService _serContract, IUnitOfWork _uow)
        {
            //cancel from system if doctor not approved
            var contractPendings = await _serContract.GetAllContractsByStatus("PENDING");
            var contractApproved = await _serContract.GetAllContractsByStatus("APPROVED");
            if(contractPendings != null || contractApproved != null)
            {
                IRepositoryBase<Contract> _repoContract;
                var currentDate = DateTime.Now;
                if (contractPendings != null)
                {
                    var tmp = contractPendings.Where(x => x.DateCreated.Date.AddDays(3) <= currentDate.Date)
                        .Select(x =>
                    {
                        x.ReasonCancel = "Sau 3 ngày không xác nhận hợp đồng với bệnh nhân.";
                        x.DateCancel = currentDate;
                        x.Status = "CANCELDS";
                        return x;
                    }).ToList();
                    if (tmp.Any())
                    {
                        _repoContract = _uow.GetRepository<Contract>();
                        if(await _repoContract.UpdateRange(tmp))
                        {
                            await _uow.CommitAsync();
                        }                        
                    }
                }
                // cancel from system of patient not active            
                if (contractApproved != null)
                {
                    var tmp = contractApproved.Where(x => x.DateApproved.GetValueOrDefault().AddDays(2) <= currentDate.Date)
                        .Select(x =>
                        {
                            x.ReasonCancel = "Sau 2 ngày không ký hợp đồng với bác sĩ.";
                            x.DateCancel = currentDate;
                            x.Status = "CANCELPS";
                            return x;
                        }).ToList();
                    if (tmp.Any())
                    {
                        _repoContract = _uow.GetRepository<Contract>();
                        if(await _repoContract.UpdateRange(tmp))
                        {
                            await _uow.CommitAsync();
                        }
                    }
                }
            }           
        }
    

    }
}
