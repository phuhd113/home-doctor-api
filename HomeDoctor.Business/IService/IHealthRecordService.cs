using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IHealthRecordService
    {
        public Task<ICollection<HealthRecordInformation>> GetHealthRecordByPatientId(int patientId,bool? onSystem);
        public Task<HealthRecordInformation> GetHealthRecordById(int healthRecordId);
        public Task<bool> CreateHealthRecord(HealthRecordCreate healthRecord);
        public Task<HealthRecordOverviewRespone> GetHROverviewByHRId(int healthRecordId);
        //// Doctor manage HealthRecord of Patient
        public Task<HealingConditions> GetHealingConditions(int healthRecordId, int contractId);
    }
}
