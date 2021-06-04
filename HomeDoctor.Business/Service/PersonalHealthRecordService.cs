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
    public class PersonalHealthRecordService : IPersonalHealthRecordService
    {
        private readonly IRepositoryBase<PersonalHealthRecord> _repo;
        private readonly IUnitOfWork _uow;

        public PersonalHealthRecordService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<PersonalHealthRecord>();
        }

        public async Task<bool> UpdatePersonalStatus(int patientId, string status)
        {
            if(patientId != 0)
            {
                var personal = await _repo.GetDbSet().Where(x => x.PatientId == patientId).FirstOrDefaultAsync();
                if(personal != null)
                {
                    if (!string.IsNullOrEmpty(status))
                    {
                        personal.PersonalStatus = status;
                        personal.DateUpdateStatus = DateTime.Now;
                        if (await _repo.Update(personal))
                        {
                            await _uow.CommitAsync();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> UpdateSmartWatchConnect(int patientId, bool isConnected)
        {
            if(patientId != 0)
            {
                var personal = await _repo.GetDbSet().Where(x => x.PatientId == patientId).FirstOrDefaultAsync();
                if(personal != null)
                {
                    personal.SmartWatchConnected = isConnected;
                    if (isConnected)
                    {
                        personal.PersonalStatus = "NORMAL";
                    }
                    else
                    {
                        personal.PersonalStatus = null;
                    }
                    var update = await _repo.Update(personal);
                    if (update)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }


    }
}
