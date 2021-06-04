using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Quartz.Jobs
{
    public class Action8AMJob : IJob
    {

        private readonly IServiceProvider _provider;

        public Action8AMJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var _serContract = scope.ServiceProvider.GetService<IContractService>();
                var _serFireBase = scope.ServiceProvider.GetService<IFirebaseFCMService>();
                var _serNoti = scope.ServiceProvider.GetService<INotificationService>();
                // ActionFirstTime
                await NotificationActionFirstTime(_serContract, _serFireBase, _serNoti);
                // noti contract locked
                //await NotificationContractLocked(_serContract, _serFireBase, _serNoti);



            }
        }
        private async Task NotificationActionFirstTime(IContractService _serContract, IFirebaseFCMService _serFireBase, INotificationService _serNoti)
        {
            var contracts = await _serContract.GetAllContractsByStatus("ACTIVE");
            if (contracts != null)
            {
                var tmp = contracts.Where(x => x.HealthRecord.AppointmentFirst == false &&
                x.HealthRecord.VitalSignScheduleFirst == false).Select(x => new
                {
                    doctorAccountId = x.Doctor.AccountId,
                    patientAccountId = x.Patient.AccountId,
                    contractId = x.ContractId,
                    dateStarted = x.DateStarted
                }).ToList();

                if (tmp.Any())
                {
                    foreach (var n in tmp)
                    {
                        // save notification 
                        var noti = new NotificationRequest()
                        {
                            AccountId = n.doctorAccountId,
                            AccountSendId = n.patientAccountId,
                            ContractId = n.contractId,
                            OnSystem = true,
                            NotificationTypeId = 15,
                        };
                        await _serNoti.InsertNotification(noti);
                        // push noti for firebase
                        await _serFireBase.PushNotification(2, n.patientAccountId, n.doctorAccountId, 15, n.contractId, null, null,null);
                    }
                }
            }
        }
    }
}
