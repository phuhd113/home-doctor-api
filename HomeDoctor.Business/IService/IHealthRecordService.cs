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
        public Task<int> CreateHealthRecord(HealthRecordCreate healthRecord);
        public Task<HealthRecordOverviewRespone> GetHROverviewByHRId(int healthRecordId);

        public Task<bool> UpdateHealthRecord(int healthRecordId, string place, string description, ICollection<string>? diseases);
        public Task<bool> UpdateActionFirstTimeToDemo(int healthRecordId, bool actionFirstTime);
        public Task<bool> DeleteHealthRecord(int healthRecordId);
    }
}
