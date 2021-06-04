using HomeDoctor.Business.IService;
using HomeDoctor.Business.UnitOfWork;
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
    public class Action06AMJob : IJob
    {
        private readonly IServiceProvider _provider;

        public Action06AMJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var currentTime = DateTime.Now;
            Console.WriteLine("Execute 6AM : "+ currentTime);
            if(currentTime.Hour == 6)
            {
                using (var scope = _provider.CreateScope())
                {
                    var _serAppointment = scope.ServiceProvider.GetService<IAppointmentService>();
                    var _serFireBase = scope.ServiceProvider.GetService<IFirebaseFCMService>();
                    var _serNoti = scope.ServiceProvider.GetService<INotificationService>();
                    var _uow = scope.ServiceProvider.GetService<IUnitOfWork>();

                    await NotifyAppointment(_uow, _serFireBase);
                }
            }                      
        }

        public static async Task NotifyAppointment(IUnitOfWork _uow, IFirebaseFCMService _serFB)
        {
            Console.WriteLine("NotifyAppointment :" + DateTime.Now);
            var _repoApp = _uow.GetRepository<Appointment>();
            var currentTime = DateTime.Now;
            var appointments = await _repoApp.GetDbSet()
                .Include(x => x.HealthRecord).ThenInclude(x => x.Contract).ThenInclude(x => x.Doctor)
                .Include(x => x.HealthRecord).ThenInclude(x => x.Contract).ThenInclude(x => x.Patient)
                .Where(x => x.DateExamination.Date == currentTime.Date && x.Status.Equals("ACTIVE"))
                .ToListAsync();
            if (appointments.Any())
            {
                Console.WriteLine("NotiAppointment :" + appointments.Count + "appointment");
                var notiDoctor = appointments.Select(x => new Notification()
                {
                    AccountId = x.HealthRecord.Contract.Doctor.AccountId,
                    AccountSendId = x.HealthRecord.Contract.Patient.AccountId,
                    AppointmentId = x.AppointmentId,
                    DateCreate = currentTime,
                    NotificationTypeId = 22,
                    OnSystem = true
                });
                var notiPatient = appointments.Select(x => new Notification()
                {
                    AccountId = x.HealthRecord.Contract.Patient.AccountId,
                    AccountSendId = x.HealthRecord.Contract.Doctor.AccountId,
                    AppointmentId = x.AppointmentId,
                    DateCreate = currentTime,
                    NotificationTypeId = 23,
                    OnSystem = true
                });
                var _repoNoti = _uow.GetRepository<Notification>();
                if(await _repoNoti.InsertRange(notiDoctor.Union(notiPatient).ToList()))
                {
                    await _uow.CommitAsync();
                    foreach(var tmp in notiDoctor)
                    {
                        await _serFB.PushNotification(2, tmp.AccountSendId.GetValueOrDefault(), tmp.AccountId, 22, null, null, tmp.AppointmentId,null);
                    }
                    foreach (var tmp in notiPatient)
                    {
                        await _serFB.PushNotification(1, tmp.AccountSendId.GetValueOrDefault(), tmp.AccountId, 23, null, null, tmp.AppointmentId,null);
                    }
                }
            }
        }
    }
}
