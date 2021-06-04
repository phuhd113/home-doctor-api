using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IVitalSignService
    {
        //public Task<bool> InsertVitalSignEveryDay(int vitalSignScheduleId);

        public Task<bool> UpdateVitalSignValue(int patientId, int vitalSignTypeId, string timeValue, string numberValue);
        public Task<VitalSignValuePatientIdRespone> GetVitalSignValueByPatientId(int patientId,int? healthRecordId, DateTime dateTime);

        public Task<VitalSignValueByMIIdRespone> GetVitalSignValueByMIId(int medicalInstructionId, int patientId);

        public Task<ICollection<VitalSignScheduleRespone>> GetVitalSignScheduleByHRId(int healthRecordId);

        public Task<ICollection<string>> GetDateTimeHaveVitalSignValue(int patientId, int? healthRecordId);

        public Task<int> ShareVitalSignValue(int healthRecordId, DateTime timeShare, int minuteShare);
        public Task<ICollection<VitalSignType>> GetVitalSignsType();
        public Task<VitalSignValueShareRespone> GetVitalSignValueShareById(int vitalSignValueShareId);
        public Task<ICollection<VitalSignValueShareRespone>> GetVitalSignValueShareByDate(int healthRecordId,DateTime? dateTime);
       
    }
}
