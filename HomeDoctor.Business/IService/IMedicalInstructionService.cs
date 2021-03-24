using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static HomeDoctor.Business.ViewModel.RequestModel.MIPresciption;

namespace HomeDoctor.Business.IService
{
    public interface IMedicalInstructionService
    {      
        public Task<bool> CreateMedicalInstructionWithImage(MedicalInstructionCreate medicalInstruction, string pathImage);
        public Task<int> CreatePrescription(MIPresciption request);
        public Task<MedicalInstructionInformation> GetMedicalInstructionById(int id);
        public Task<ICollection<MedicalInstructionByDiseaseRespone>> GetMIToCreateContract(int patientId,ICollection<string> diseaseIds,int medicalInstructionType);
        public Task<ICollection<MedicalInstructionInformation>> GetPrescriptionByPatientId(int patientId,int? healthRecordId);
        public Task<ICollection<MedicalInstructionInformation>> GetMedicalInstructionsByHRId(int healthRecordId);

        public Task<int> UpdatePrecription(int medicalInstructionId, string? status,string? reasonCancel, ICollection<MedicationSchedule>? medicationSchedules);

        public Task<ICollection<MedicalInstructionToShareRespone>> GetMedicalInstructionToShare(int patientId, int contractId);
        public Task<int> CreateVitalSignSchedule(MIVitalSignSchedule vitalSign);
    }
}
