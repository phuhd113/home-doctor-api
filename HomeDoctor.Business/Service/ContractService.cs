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
        private readonly IRepositoryBase<Contract> _repo;
        private readonly IUnitOfWork _uow;

        public ContractService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Contract>();
        }

        public async Task<bool> CreateContractByPatient(ContractCreation contractCre, PatientInformation patient, DoctorInformation doctor, License license, ICollection<Disease> diseases)
        {
            if (contractCre != null)
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
                    DateCreated = DateTime.UtcNow.ToLocalTime().AddHours(-1),
                    DateStarted = contractCre.DateStarted,
                    DateFinished = contractCre.DateStarted.AddDays(license.Days),
                    Note = contractCre.Note,
                    Status = "PENDING",
                    LicenseId = license.LicenseId,
                    DaysOfTracking = license.Days,
                    NameLicense = license.Name,
                    PriceLicense = license.Price,
                    Diseases = diseases
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

        public async Task<ICollection<ContractInformation>> GetContractsByStatus(int? doctorId, int? patientId, string? status)
        {
            if (!String.IsNullOrEmpty(status))
            {
                var contracts = _repo.GetDbSet().
                    Where(x => (doctorId != null ? doctorId == x.DoctorId : patientId == x.PatientId) && x.Status.Equals(status.ToUpper())).Include(x => x.Diseases).
                    Include(x => x.Doctor).Include(x => x.Patient).ThenInclude(x => x.Account).
                    OrderBy(x => x.DateCreated).
                    Select(x => new ContractInformation()
                    {
                        //contract
                        ContractId = x.ContractId,
                        ContractCode = x.ContractCode,
                        DateCreated = x.DateCreated,
                        DateFinished = x.DateFinished,
                        DateStarted = x.DateStarted,
                        Status = x.Status,
                        //license
                        NameLicense = x.NameLicense,
                        PriceLicense = x.PriceLicense,
                        DaysOfTracking = x.DaysOfTracking,
                        //Doctor
                        FullNamePatient = x.Patient.Account.FullName,
                        PhoneNumberPatient = x.Patient.Account.PhoneNumber,
                        //Patient
                        FullNameDoctor = x.Doctor.Account.FullName,
                        PhoneNumberDoctor = x.Doctor.Account.PhoneNumber,
                        //Disease
                        Diseases = x.Diseases,
                        Note = x.Note                    
                    });
                if (contracts.Count() != 0)
                {
                    return contracts.ToList();
                }
            }
            else
            {
                var contracts = _repo.GetDbSet().
                   Where(x => (doctorId != null ? doctorId == x.DoctorId : patientId == x.PatientId)).
                   Include(x => x.Doctor).Include(x => x.Patient).ThenInclude(x => x.Account).
                   OrderBy(x => x.DateCreated).
                   Select(x => new ContractInformation()
                   {
                       //contract
                       ContractId = x.ContractId,
                       ContractCode = x.ContractCode,
                       DateCreated = x.DateCreated,
                       DateFinished = x.DateFinished,
                       DateStarted = x.DateStarted,
                       Status = x.Status,
                       //license
                       NameLicense = x.NameLicense,
                       PriceLicense = x.PriceLicense,
                       DaysOfTracking = x.DaysOfTracking,
                       //Doctor
                       FullNamePatient = x.Patient.Account.FullName,
                       PhoneNumberPatient = x.Patient.Account.PhoneNumber,
                       //Patient
                       FullNameDoctor = x.Doctor.Account.FullName,
                       PhoneNumberDoctor = x.Doctor.Account.PhoneNumber,
                       //Disease
                       Diseases = x.Diseases,
                       Note = x.Note
                   });
                if (contracts.Count() != 0)
                {
                    return contracts.ToList();
                }
            }
            return null;
        }
        public async Task<bool> CheckContractToCreateNew(int doctorId, int patientId)
        {
            if (doctorId != 0 && patientId != 0)
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
            string contractCode = "HDR" + dateTime.Year.ToString() +
                dateTime.Month.ToString() +
                dateTime.Day.ToString() +
                doctorId + patientId;
            return contractCode;
        }

        public async Task<bool> UpdateStatuByDoctor(int contractId, DateTime? dateStarted, int? daysOfTracking, string status)
        {
            if (contractId != 0)
            {
                var contract = _repo.GetById(contractId).Result;
                if (contract != null)
                {
                    if (dateStarted != null)
                    {
                        contract.DateStarted = dateStarted.Value;
                        contract.DateFinished = contract.DateStarted.AddDays(contract.DaysOfTracking);
                    }

                    if (daysOfTracking != null)
                    {
                        contract.DaysOfTracking = daysOfTracking.Value;
                        contract.DateFinished = contract.DateStarted.AddDays(daysOfTracking.Value);
                    }
                    if (!string.IsNullOrEmpty(status) && !contract.Status.Equals(status.ToUpper()))
                    {
                        contract.Status = status.ToUpper();
                    }
                    var check = await _repo.Update(contract);
                    if (check)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<Contract> GetContractByContractId(int? contractId)
        {
            if (contractId != 0)
            {
                var contract = _repo.GetById(contractId).Result;
                if (contract != null)
                {
                    return contract;
                }
            }
            return null;
        }
    }
}
