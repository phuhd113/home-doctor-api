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
    public class MedicalInstructionTypeService : IMedicalInstructionTypeService
    {
        private readonly IRepositoryBase<MedicalInstructionType> _repo;
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IUnitOfWork _uow;

        public MedicalInstructionTypeService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<MedicalInstructionType>();
            _repoMI = _uow.GetRepository<MedicalInstruction>();
        }

        public async Task<MedicalInstructionType> GetMedicalInstructionTypeById(int medicalInstructionTypeId)
        {
            if(medicalInstructionTypeId != 0)
            {
                var mit = await _repo.GetById(medicalInstructionTypeId);
                if(mit != null)
                {
                    return mit;
                }
            }
            return null;
        }

        public async Task<ICollection<MedicalInstructionType>> GetMedicalInstructionTypeByStatus(string status)
        {
            {
                // if status is empty or 'ACTIVE 'then types is get with status active.

                var MITypes = await _repo.GetDbSet().Where(x => !string.IsNullOrEmpty(status) ? x.Status.Equals(status.ToUpper()) : true).ToListAsync();
                if (MITypes.Any())
                {
                    return MITypes;
                }
                return null;
            }
        }

        public async Task<ICollection<MedicalInstructionType>> GetMITypeOfPatientToShare(int patientId,string? diseaseId, ICollection<int> medicalInstructionIds)
        {
            if(patientId != 0)
            {
                if (medicalInstructionIds.Any())
                {
                    medicalInstructionIds = medicalInstructionIds.Distinct().ToList();
                }
                var medicalInstructionTypes = await _repoMI.GetDbSet().Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId 
                && x.MedicalInstructionTypeId != 10 && !medicalInstructionIds.Any(y => y == x.MedicalInstructionId) &&
                (!string.IsNullOrEmpty(diseaseId) ? x.Diseases.Any(y => y.DiseaseId.Equals(diseaseId)) : true))
                    .Select(x => new
                {
                    x.MedicalInstructionTypeId,
                    x.MedicalInstructionType.Name
                }).Distinct().Select(x => new MedicalInstructionType() {
                    MedicalInstructionTypeId = x.MedicalInstructionTypeId,
                    Name = x.Name
                }).ToListAsync();
                if (medicalInstructionTypes.Any())
                {                   
                    return medicalInstructionTypes;
                }
            }
            return null;
        }
    }
}
