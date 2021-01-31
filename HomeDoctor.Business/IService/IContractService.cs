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
        public Task<bool> CreateContractByPatient(ContractCreation contract, PatientInformation patient, DoctorInformation doctor, License license, ICollection<Disease> diseases);

        public Task<ICollection<ContractInformation>> GetContractsByStatus(int? doctorId, int? patientId,string? status);
        public Task<Contract> GetContractByContractId(int? contractId);
        public Task<bool> UpdateStatuByDoctor(int contractId, DateTime? dateStarted, int? daysOfTracking, string status);
        public Task<bool> CheckContractToCreateNew(int doctorId, int patientId);
    }
}
