using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class ActionService : IActionService
    {
        private readonly IRepositoryBase<HealthRecord> _repoHR;
        private readonly IUnitOfWork _uow;

        public ActionService(IUnitOfWork uow)
        {
            _uow = uow;
            _repoHR = _uow.GetRepository<HealthRecord>();
        }

        public async Task<bool> CheckActionFirstTime(int healthRecordId, bool? appointmentFirst, bool? vitalSignScheduleFirst)
        {
            if(healthRecordId != 0)
            {
                var check = await _repoHR.GetDbSet().Where(x => x.HealthRecordId == healthRecordId).AnyAsync(
                    x => (appointmentFirst != null ? x.AppointmentFirst == appointmentFirst : true)
                    && (vitalSignScheduleFirst != null ? x.VitalSignScheduleFirst == vitalSignScheduleFirst : true));
                return check;
            }
            return false;
        }

        public async Task<bool> UpdateActionFirstTime(int healthRecordId, bool? appointmentFirst, bool? vitalSignScheduleFirst)
        {
           if(healthRecordId != 0)
            {
                if(appointmentFirst != null || vitalSignScheduleFirst != null)
                {
                    var action = await _repoHR.GetDbSet().Where(x => x.HealthRecordId == healthRecordId).FirstOrDefaultAsync();
                    if(action != null)
                    {
                        if(appointmentFirst != null)
                        {
                            action.AppointmentFirst = appointmentFirst.GetValueOrDefault();
                        }
                        if(vitalSignScheduleFirst != null)
                        {
                            action.VitalSignScheduleFirst = vitalSignScheduleFirst.GetValueOrDefault();
                        }
                        if(await _repoHR.Update(action))
                        {
                            await _uow.CommitAsync();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
