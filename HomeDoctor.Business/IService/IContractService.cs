using HomeDoctor.Business.ViewModel;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static HomeDoctor.Business.ViewModel.RequestModel.ContractUpdateRequest;

namespace HomeDoctor.Business.IService
{
    public interface IContractService
    {
        public Task<int> CreateContractByPatient(ContractCreation contract, PatientInformation patient, DoctorInformation doctor);

        public Task<ICollection<ContractInformation>> GetContractsByStatus(int? doctorId, int? patientId,string? status);
        public Task<ContractDetailInformation> GetContractByContractId(int? contractId);
        public Task<bool> UpdateStatusContract(int contractId, DateTime? dateStarted, int? daysOfTracking, string status, ICollection<int>? medicalInstructionChooses);
        public Task<string> CheckContractToCreateNew(int doctorId, int patientId,DateTime? dateStart);

        /// Service of Job Quartz
        public  Task<ICollection<Contract>> GetAllContractsByStatus(string status);

        public Task<ICollection<ContractInformation>> GetAllContractsByAdmin(string status);

        public Task<bool> UpdateContractToDemo(int contractId, string status, DateTime? TimeStarted);


    }
}
