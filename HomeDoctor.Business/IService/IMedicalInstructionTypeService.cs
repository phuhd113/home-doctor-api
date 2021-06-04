using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IMedicalInstructionTypeService
    {
        public Task<ICollection<MedicalInstructionType>> GetMedicalInstructionTypeByStatus(string? status);

        public Task<ICollection<MedicalInstructionType>> GetMITypeOfPatientToShare(int patientId, string? diseaseId, ICollection<int> medicalInstructionIds);
        public Task<MedicalInstructionType> GetMedicalInstructionTypeById(int medicalInstructionTypeId);
    }
}
