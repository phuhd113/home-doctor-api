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
    public class ActionFirstTimeJob : IJob
    {
       
        private readonly IServiceProvider _provider;

        public ActionFirstTimeJob(IServiceProvider provider)
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
                var contracts = await _serContract.GetAllContractsByStatus("ACTIVE");
                if (contracts != null)
                {
                    var tmp = contracts.Where(x => x.ActionFirstTime.AppointmentFirst == false && x.ActionFirstTime.PrescriptionFirst == false).Select(x => new
                    {
                        doctorAccountId = x.Doctor.AccountId,
                        patientAccountId = x.Patient.AccountId
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
                                OnSystem = true,
                                NotificationTypeId = 15,
                            };
                            await _serNoti.InsertNotification(noti);
                            // push noti for firebase
                            await _serFireBase.PushNotification(2, n.patientAccountId, n.doctorAccountId, 15, null, null);
                        }
                    }
                }
            }
        }
    }
}
