using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IContractService
    {
        public Task<bool> CreateContractByPatient(ContractCreation contract);

        public Task<ICollection<ContractInformation>> GetContractOfDoctorByStatus(int doctorId, string status);
    }
}
