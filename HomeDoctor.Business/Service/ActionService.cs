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
        private readonly IRepositoryBase<ActionFirstTime> _repo;
        private readonly IUnitOfWork _uow;

        public ActionService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<ActionFirstTime>();
        }

        public async Task<bool> CheckActionFirstTime(int contractId, bool? appointmentFirst, bool? prescriptionFirst)
        {
            if(contractId != 0)
            {
                var check = await _repo.GetDbSet().Where(x => x.ContractId == contractId).AnyAsync(
                    x => (appointmentFirst != null ? x.AppointmentFirst == appointmentFirst : true)
                    && (prescriptionFirst != null ? x.PrescriptionFirst == prescriptionFirst : true));
                return check;
            }
            return false;
        }

        public async Task<bool> UpdateActionFirstTime(int contractId, bool? appointmentFirst, bool? prescriptionFirst)
        {
           if(contractId != 0)
            {
                if(appointmentFirst != null || prescriptionFirst != null)
                {
                    var action = await _repo.GetDbSet().Where(x => x.ContractId == contractId).FirstOrDefaultAsync();
                    if(action != null)
                    {
                        if(appointmentFirst != null)
                        {
                            action.AppointmentFirst = appointmentFirst.GetValueOrDefault();
                        }
                        else
                        {
                            action.PrescriptionFirst = prescriptionFirst.GetValueOrDefault();
                        }
                        if(await _repo.Update(action))
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
