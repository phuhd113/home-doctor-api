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
        private readonly IRepositoryBase<HealthRecord> _repoHR;
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<Disease> _repoDisease;
        private readonly ILicenseService _serLicense;

        public ContractService(IUnitOfWork uow, ILicenseService serLicense)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Contract>();
            _repoHR = _uow.GetRepository<HealthRecord>();
            _repoDisease = _uow.GetRepository<Disease>();
            _serLicense = serLicense;
        }

        public async Task<int> CreateContractByPatient(ContractCreation contractCre, PatientInformation patient, DoctorInformation doctor)
        {
            if (contractCre != null)
            {
                // Insert medicalinstructionShare when patient create contract             
                List<MedicalInstructionShare> medicalInstructionShares = null;
                List<Disease> listDisease = null;
                // Add Disease of HealthRecord
                if (contractCre.DiseaseIds.Count != 0)
                {
                    listDisease = new List<Disease>();
                    foreach (var n in contractCre.DiseaseIds)
                    {
                        //Get disease to insert contract
                        var disease = await _repoDisease.GetById(n);
                        if (disease != null) listDisease.Add(disease);
                    }
                }
                // Add list medicalInstructionShare for HR
                if (contractCre.MedicalInstructionIds.Count != 0)
                {
                    medicalInstructionShares = new List<MedicalInstructionShare>();
                    foreach (var miId in contractCre.MedicalInstructionIds)
                    {
                        var tmp = new MedicalInstructionShare()
                        {
                            MedicalInstructionId = miId,
                            Status = "CONTRACT",
                        };
                        medicalInstructionShares.Add(tmp);
                    }
                }
                Contract contract = new Contract()
                {
                    DoctorId = contractCre.DoctorId,
                    FullNameDoctor = doctor.FullName,
                    DOBDoctor = doctor.DateOfBirth,
                    PhoneNumberDoctor = doctor.Phone,
                    WorkLocationDoctor = doctor.WorkLocation,
                    AddressDoctor = doctor.Address,
                    PatientId = contractCre.PatientId,
                    AddressPatient = patient.Address,
                    DOBPatient = patient.DateOfBirth,
                    FullNamePatient = patient.FullName,
                    PhoneNumberPatient = patient.PhoneNumber,
                    ContractCode = this.GenerateContractCode(contractCre.DoctorId, contractCre.PatientId),
                    DateCreated = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7),
                    DateStarted = contractCre.DateStarted,
                    Note = contractCre.Note,
                    Status = "PENDING",
                    Diseases = listDisease,
                    MedicalInstructionShares = medicalInstructionShares,

                };
                var check = await _repo.Insert(contract);
                if (check)
                {
                    await _uow.CommitAsync();
                    return contract.ContractId;
                }
            }
            return 0;
        }

        public async Task<ICollection<ContractInformation>> GetContractsByStatus(int? doctorId, int? patientId, string? status)
        {
            if (!String.IsNullOrEmpty(status))
            {


                var contracts = await _repo.GetDbSet().
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
                    }).ToListAsync();
                if (contracts.Count != 0)
                {
                    return contracts;
                }
            }
            else
            {
                var contracts = await _repo.GetDbSet().
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
                   }).ToListAsync();
                if (contracts.Count != 0)
                {
                    return contracts;
                }
            }
            return null;
        }
        public async Task<string> CheckContractToCreateNew(int doctorId, int patientId)
        {
            if (doctorId != 0 && patientId != 0)
            {
                var tmp = await _repo.GetDbSet().Where(x => x.DoctorId == doctorId && x.PatientId == patientId &&(x.Status.Equals("PENDING") || x.Status.Equals("ACTIVE") || x.Status.Equals("APPROVED"))).
                    Select(x => x.Status).FirstOrDefaultAsync();
                return tmp;
            }
            return null;
        }
        // Create ContractCode with DoctorId and PatientId
        private string GenerateContractCode(int doctorId, int patientId)
        {
            var dateTime = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7);
            string contractCode = "HDR" + dateTime.Year.ToString() +
                dateTime.Month.ToString() +
                dateTime.Day.ToString() +
                doctorId + patientId;
            return contractCode;
        }

        public async Task<bool> UpdateStatuContract(int contractId, DateTime? dateStarted, int? daysOfTracking, string status)
        {
            if (contractId != 0)
            {
                var contract = _repo.GetDbSet().Include(x => x.Patient.PersonalHealthRecord).Include(x => x.Diseases).Where(x => x.ContractId == contractId).FirstOrDefault();
                if (contract != null)
                {
                    // Update DateStarted when doctor approve
                    if (dateStarted != null)
                    {
                        contract.DateStarted = dateStarted.Value;
                        contract.DateFinished = contract.DateStarted.AddDays(contract.DaysOfTracking);
                    }
                    // Update DaysOfTracking when doctor approve
                    if (daysOfTracking != null)
                    {
                        var license = await _serLicense.GetLicenseByDays(daysOfTracking.Value);
                        if (license != null)
                        {
                            contract.LicenseId = license.LicenseId;
                            contract.NameLicense = license.Name;
                            contract.PriceLicense = license.Price;
                        }
                        contract.DaysOfTracking = daysOfTracking.Value;
                        contract.DateFinished = contract.DateStarted.AddDays(daysOfTracking.Value);
                    }
                    if (!string.IsNullOrEmpty(status) && !contract.Status.Equals(status.ToUpper()))
                    {
                        contract.Status = status.ToUpper();
                    }
                                     
                    if (status.ToUpper().Equals("ACTIVE"))
                    {
                        var dateNow = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7);
                        // Create Healthrecord between Doctor and Patient when patient sign 
                        var healthRecord = new HealthRecord()
                        {
                            ContractId = contract.ContractId,
                            DateCreated = dateNow,
                            Diseases = contract.Diseases,
                            PersonalHealthRecordId = contract.Patient.PersonalHealthRecord.PersonalHealthRecordId,
                            Place = "Bác sĩ " + contract.FullNameDoctor
                        };
                        var addHealRecord = await _repoHR.Insert(healthRecord);
                        // Create ActionFirstTime When Patient sign Contract
                        
                        var actionFirst = new ActionFirstTime()
                        {
                            AppointmentFirst = false,
                            PrescriptionFirst = false,
                            ContractId = contract.ContractId,
                            DateCreated = dateNow,
                            ActionEveryWeeks = new List<ActionEveryWeek>() {
                                new ActionEveryWeek(){
                                    AppointmentWeek = false,
                                    VitalSignWeek = false,
                                    PrescriptionWeek = false,
                                    DateCreated = dateNow,
                                    ActionEveryDays = new List<ActionEveryDay>()
                                    {
                                        new ActionEveryDay()
                                        {
                                            DateCreated = dateNow,
                                            Examination = null
                                        }
                                    }
                                } }
                        };
                    }
                    // Update contract
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

        public async Task<ContractDetailInformation> GetContractByContractId(int? contractId)
        {
            if (contractId != 0)
            {
                var contract = await _repo.GetDbSet().Where(x => x.ContractId == contractId).Include(x => x.Diseases).Include(x => x.Doctor).Include(x => x.Patient).ThenInclude(x => x.Account).Include(x => x.MedicalInstructionShares).ThenInclude(x => x.MedicalInstruction).ThenInclude(x => x.MedicalInstructionType).FirstOrDefaultAsync();

                if (contract != null)
                {
                    var respone = new ContractDetailInformation()
                    {
                        AccountDoctorId = contract.Doctor.AccountId,
                        AccountPatientId = contract.Patient.AccountId,
                        DoctorId = contract.DoctorId,
                        FullNameDoctor = contract.FullNameDoctor,
                        AddressDoctor = contract.AddressDoctor,
                        Experience = contract.Doctor.Experience,
                        Specialization = contract.Doctor.Specialization,
                        PhoneNumberDoctor = contract.PhoneNumberDoctor,
                        DOBDoctor = contract.DOBDoctor,
                        WorkLocationDoctor = contract.WorkLocationDoctor,
                        PatientId = contract.PatientId,
                        FullNamePatient = contract.FullNamePatient,
                        AddressPatient = contract.AddressPatient,
                        GenderPatient = contract.Patient.Account.Gender,
                        PhoneNumberPatient = contract.PhoneNumberPatient,
                        DOBPatient = contract.DOBPatient,
                        ContractCode = contract.ContractCode,
                        DateCreated = contract.DateCreated,
                        DateStarted = contract.DateStarted,
                        DateFinished = contract.DateFinished,
                        DaysOfTracking = contract.DaysOfTracking,
                        Status = contract.Status,
                        Note = contract.Note,
                        Diseases = contract.Diseases.Select(x => new ContractDetailInformation.Disease()
                        {
                            DiseaseId = x.DiseaseId,
                            NameDisease = x.Name
                        }).ToList(),
                        NameLicense = contract.NameLicense,
                        PriceLicense = contract.PriceLicense,
                        MedicalInstructionTypes = contract.MedicalInstructionShares.GroupBy(y => y.MedicalInstruction.MedicalInstructionType.Name).Select(z => new ContractDetailInformation.MedicalInstructionType()
                        {

                            MedicalInstructionTypeName = z.Key,
                            MedicalInstructions = z.Select(m => new ContractDetailInformation.MedicalInstruction()
                            {
                                Image = m.MedicalInstruction.Image,
                                Diagnose = m.MedicalInstruction.Diagnose,
                                Description = m.MedicalInstruction.Description
                            }).ToList()
                        }).ToList()
                    };
                    return respone;
                }
            }
            return null;
        }

        public async Task<ICollection<Contract>> GetAllContractsByStatus(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                var contract = await _repo.GetDbSet().Where(x => x.Status.Equals(status)).Include(x => x.ActionFirstTime).Include(x => x.Doctor).Include(x => x.Patient).ToListAsync();
                if (contract.Any())
                {
                    return contract;
                }
            }
            return null;
        }
    }
}
