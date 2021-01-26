using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class ContractService : IContractService
    {
        private IRepositoryBase<Contract> _repo;
        private readonly IUnitOfWork _uow;

        public ContractService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Contract>();
        }

        public async Task<bool> CreateContractByPatient(ContractCreation contractCre)
        {
            if(contractCre != null)
            {
                Contract contract = new Contract()
                {
                    DoctorId = contractCre.DoctorId,
                    PatientId = contractCre.PatientId,
                    DateCreated = DateTime.UtcNow,
                    DateStarted = contractCre.DateStarted,
                    DateFinished = contractCre.DateStarted.AddDays(contractCre.DaysOfTracking),
                    Reason = contractCre.Reason,
                    Status = "PENDING",
                    DaysOfTracking =contractCre.DaysOfTracking
                };
                 var check = await _repo.Insert(contract);
                if (check)
                {
                    await _uow.CommitAsync();
                    return true;
                }              
            }
            return false;
        }

        public async Task<ICollection<ContractInformation>> GetContractOfDoctorByStatus(int doctorId, string status)
        {
            if(doctorId != 0 && !String.IsNullOrEmpty(status))
            {
                var contracts = _repo.GetDbSet().
                    Where(x => x.DoctorId == doctorId && x.Status.Equals(status.ToUpper())).
                    Include(x => x.Doctor).Include(x => x.Patient).ThenInclude(x => x.Account).
                    OrderBy(x => x.DateCreated).
                    Select(x => new ContractInformation() {
                    ContractCode ="",
                    DateCreated = x.DateCreated,
                    DaysOfTracking = x.DaysOfTracking,
                    FullName = x.Patient.Account.FullName,
                    PatientId = x.PatientId,
                    PhoneNumber = x.Patient.Account.PhoneNumber,
                    Reason = x.Reason,
                    Status = x.Status
                    });
                if(contracts.Count() != 0)
                {
                    return contracts.ToList();
                }
            }
            return null;
        }
    }
}
