using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel;
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

        public async Task<bool> CreateContractByPatient(ContractCreation contractCre,PatientInformation patient, DoctorInformation doctor)
        {
            if(contractCre != null)
            {
                Contract contract = new Contract()
                {
                    DoctorId = contractCre.DoctorId,
                    FullNameDoctor = doctor.FullName,
                    DOBDoctor = doctor.DateOfBirth,
                    PhoneNumberDoctor = doctor.Phone,
                    WorkLocationDoctor = doctor.WorkLocation,                    
                    PatientId = contractCre.PatientId,
                    AddressPatient = patient.Address,
                    DOBPatient = patient.DateOfBirth,
                    FullNamePatient = patient.FullName,
                    PhoneNumberPatient = patient.PhoneNumber,
                    ContractCode = this.GenerateContractCode(contractCre.DoctorId, contractCre.PatientId),
                    DateCreated = DateTime.UtcNow.ToLocalTime(),
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
                    ContractCode =x.ContractCode,
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
        public async Task<bool> CheckContractToCreateNew(int doctorId, int patientId)
        {
            if(doctorId != 0 && patientId != 0)
            {
                var tmp = _repo.GetDbSet().Where(x => x.DoctorId == doctorId && x.PatientId == patientId).
                    Any(x => x.Status.Equals("PENDING") || x.Status.Equals("ACTIVE"));
                return tmp;
            }
            return false;
        }

        public string GenerateContractCode(int doctorId, int patientId)
        {
            var dateTime = DateTime.UtcNow.ToLocalTime();
            string contractCode = "HDR"+dateTime.Year.ToString() +
                dateTime.Month.ToString() +
                dateTime.Day.ToString() +
                doctorId + patientId;
            return contractCode;
        }
    }
}
