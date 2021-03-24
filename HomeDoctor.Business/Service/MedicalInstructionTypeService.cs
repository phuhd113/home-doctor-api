using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class MedicalInstructionTypeService : IMedicalInstructionTypeService
    {
        private readonly IRepositoryBase<MedicalInstructionType> _repo;
        private readonly IUnitOfWork _uow;

        public MedicalInstructionTypeService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<MedicalInstructionType>();
        }

        public async Task<ICollection<MedicalInstructionType>> GetMedicalInstructionTypeByStatus(string status)
        {
            {
                // if status is empty or 'ACTIVE 'then types is get with status active.
                if (!String.IsNullOrEmpty(status))
                {
                    var MITypes = _repo.GetDbSet().Where(x => x.Status.Equals(status.ToUpper())).ToList();
                    if(MITypes.Count() != 0)
                    {
                        return MITypes;
                    }
                }
                return null;
            }
        }
    }
}
