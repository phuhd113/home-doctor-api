using HomeDoctor.Business.ViewModel;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IContractService
    {
        public Task<int> CreateContractByPatient(ContractCreation contract, PatientInformation patient, DoctorInformation doctor);

        public Task<ICollection<ContractInformation>> GetContractsByStatus(int? doctorId, int? patientId,string? status);
        public Task<ContractDetailInformation> GetContractByContractId(int? contractId);
        public Task<bool> UpdateStatuContract(int contractId, DateTime? dateStarted, int? daysOfTracking, string status);
        public Task<string> CheckContractToCreateNew(int doctorId, int patientId);

        /// Service of Job Quartz
        public Task<ICollection<Contract>> GetAllContractsByStatus(string status);


    }
}
