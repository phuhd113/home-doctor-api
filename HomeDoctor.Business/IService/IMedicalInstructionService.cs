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
        public Task<bool> CreateMedicalInstructionWithImage(MedicalInstructionCreate medicalInstruction, ICollection<string> pathImages, string fromBy);
        public Task<int> CreatePrescription(MIPresciption request);
        public Task<MedicalInstructionInformation> GetMedicalInstructionById(int id);
        public Task<ICollection<MedicalInstructionByDiseaseRespone>> GetMIToCreateContract(int patientId, string diseaseId, int? medicalInstructionTypeId, ICollection<int> medicalInstructionIds);
        public Task<ICollection<MedicalInstructionInformation>> GetPrescriptionByPatientId(int patientId,int? healthRecordId);
        public Task<ICollection<MedicalInstructionOverviewRespone>> GetMedicalInstructionsByHRId(int healthRecordId,int? medicalInstructionTypeId);

        public Task<int> UpdatePrecription(int medicalInstructionId, string? status,string? reasonCancel, ICollection<MedicationSchedule>? medicationSchedules);

        public Task<ICollection<MedicalInstructionToShareRespone>> GetMedicalInstructionsToShare(int patientId, int healthRecordId,int? medicalInstructionTypeId);
        public Task<int> CreateVitalSignSchedule(MIVitalSignSchedule vitalSign);

        public Task<bool> ShareMedicalInstructions(int healthRecordId, ICollection<int> medicalInstructionIds);
        public Task<ICollection<VitalSignScheduleRespone>> GetVitalSignScheduleByPatientId(int patientId, string? status);
        public Task<bool> DeleteMedicalInstruction(int medicalInstructionId);

        public Task<bool> AddMedicalInstructionFromContract(int contractId, ICollection<int> medicalInstructionIds);
        public Task<bool> UpdateStatusMedicalInstruction(int medicalInstructionId, string status);
    }
}
