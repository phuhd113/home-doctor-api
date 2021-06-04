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
using static HomeDoctor.Business.ViewModel.RequestModel.ContractUpdateRequest;

namespace HomeDoctor.Business.Service
{
    public class ContractService : IContractService
    {
        private readonly IRepositoryBase<Contract> _repo;
        private readonly IRepositoryBase<HealthRecord> _repoHR;
        private readonly IRepositoryBase<Disease> _repoDisease;
        private readonly IRepositoryBase<License> _repoLicense;
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IUnitOfWork _uow;


        public ContractService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Contract>();
            _repoHR = _uow.GetRepository<HealthRecord>();
            _repoDisease = _uow.GetRepository<Disease>();
            _repoLicense = _uow.GetRepository<License>();
            _repoMI = _uow.GetRepository<MedicalInstruction>();
        }

        public async Task<int> CreateContractByPatient(ContractCreation contractCre, PatientInformation patient, DoctorInformation doctor)
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
                    AddressDoctor = doctor.Address,
                    PatientId = contractCre.PatientId,
                    AddressPatient = patient.Address,
                    DOBPatient = patient.DateOfBirth,
                    FullNamePatient = patient.FullName,
                    PhoneNumberPatient = patient.PhoneNumber,
                    ContractCode = this.GenerateContractCode(contractCre.DoctorId, contractCre.PatientId),
                    DateCreated = DateTime.Now,
                    DateStarted = contractCre.DateStarted,
                    Note = contractCre.Note,
                    Status = "PENDING",
                    Diseases = await _repoDisease.GetDbSet().Where(x => contractCre.DiseaseHealthRecordIds.Any(y => y.Equals(x.DiseaseId))).ToListAsync()
                };

                if (contractCre.DiseaseMedicalInstructions.Any())
                {
                    var contractMedicalInstructions = new List<ContractMedicalInstruction>();
                    foreach (var miIds in contractCre.DiseaseMedicalInstructions)
                    {
                        foreach (var mi in miIds.MedicalInstructionIds)
                        {
                            // Get disease of listmedicalInstruction
                            var tmp2 = await _repoMI.GetDbSet().Include(x => x.Diseases)
                                .Include(x => x.HealthRecord.Diseases)
                                .Where(x => x.MedicalInstructionId == mi).FirstOrDefaultAsync();
                            if (tmp2 != null)
                            {
                                var tmp = new ContractMedicalInstruction()
                                {
                                    DiseaseChoosedId = !string.IsNullOrEmpty(miIds.DiseaseId) ? miIds.DiseaseId : null, 
                                    DiseaseIds = tmp2.Diseases.Any() ? tmp2.Diseases.Aggregate<Disease,string>("",(str,disease) => str += disease.DiseaseId+",") : null,
                                    MedicalInstructionId = mi,
                                    Conclusion = tmp2.Conclusion,
                                    Description = tmp2.Description,
                                };
                                if(!tmp2.Diseases.Any())
                                {
                                    tmp.DiseaseIds = tmp2.HealthRecord.Diseases.Aggregate<Disease,string>("",(str,disease) => str += disease.DiseaseId+",");
                                }
                                contractMedicalInstructions.Add(tmp);
                            }

                        }
                    }
                    if (contractMedicalInstructions.Any())
                    {
                        contract.ContractMedicalInstructions = contractMedicalInstructions;
                    }
                }
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
                        PatientId = x.PatientId,
                        FullNameDoctor = x.Doctor.Account.FullName,
                        PhoneNumberDoctor = x.Doctor.Account.PhoneNumber,
                        //Disease
                        Diseases = x.Diseases.Select(y => new ContractInformation.Disease()
                        {
                            DiseaseId = y.DiseaseId,
                            Name = y.Name
                        }).ToList(),
                        Note = x.Note
                    }).ToListAsync();
                if (contracts.Any())
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
                       PatientId = x.PatientId,
                       FullNameDoctor = x.Doctor.Account.FullName,
                       PhoneNumberDoctor = x.Doctor.Account.PhoneNumber,
                       //Disease
                       Diseases = x.Diseases.Select(y => new ContractInformation.Disease()
                       {
                           DiseaseId = y.DiseaseId,
                           Name = y.Name
                       }).ToList(),
                       Note = x.Note
                   }).ToListAsync();
                if (contracts.Any())
                {
                    return contracts;
                }
            }
            return null;
        }
        public async Task<string> CheckContractToCreateNew(int doctorId, int patientId, DateTime? dateStart)
        {
            if (doctorId != 0 && patientId != 0)
            {
                if (dateStart == null)
                {
                    // Get contract between doctor and patient != finished and unsighed
                    var tmp = await _repo.GetDbSet().Where(x => x.DoctorId == doctorId && x.PatientId == patientId && !x.Status.Equals("FINISHED") && !x.Status.Equals("UNSIGNED") && !x.Status.Equals("CANCELP") && !x.Status.Equals("CANCELD")).
                    Select(x => new { x.Status, x.DateFinished }).FirstOrDefaultAsync();
                    if (tmp != null)
                    {
                        if (tmp.Status.Equals("ACTIVE") || tmp.Status.Equals("APPROVED") || tmp.Status.Equals("SIGNED"))
                        {
                            return tmp.DateFinished.ToString("dd/MM/yyyy");
                        }
                        return tmp.Status;
                    }
                }
                else
                
                {
                    var tmp = await _repo.GetDbSet().Where(x => x.DoctorId == doctorId && x.Status.Equals("ACTIVE")).OrderBy(x => x.DateFinished.Date).Select(x => x.DateFinished).ToListAsync();
                    if (tmp.Count > 5)
                    {
                        if (dateStart.GetValueOrDefault().Date <= tmp.FirstOrDefault().Date)
                        {
                            return tmp.FirstOrDefault().Date.ToString("dd/MM/yyyy");
                        }
                    }

                }
            }
            return null;
        }
        // Create ContractCode with DoctorId and PatientId
        private string GenerateContractCode(int doctorId, int patientId)
        {
            var dateTime = DateTime.Now;
            string contractCode = "HDR" + dateTime.Year.ToString() +
                dateTime.Month.ToString() +
                dateTime.Day.ToString() +
                doctorId + patientId;
            return contractCode;
        }

        public async Task<bool> UpdateStatusContract(int contractId, DateTime? dateStarted, int? daysOfTracking, string status, ICollection<int>? medicalInstructionChooses)
        {
            if (contractId != 0)
            {
                var contract = await _repo.GetDbSet().Include(x => x.Patient.PersonalHealthRecord)
                    .Include(x => x.Diseases)
                    .Include(x => x.ContractMedicalInstructions).ThenInclude(x => x.MedicalInstruction)
                    .ThenInclude(x => x.MedicalInstructionImages)
                    .Include(x => x.HealthRecord)
                    .Where(x => x.ContractId == contractId).FirstOrDefaultAsync();
                if (contract != null)
                {
                    /*
                    // Update Status of medicalInstructionShare to CHOOSE                                        
                    if (medicalInstructionChooseIds != null)
                    {
                        var miShares = await _repoMIShare.GetDbSet().Where(x => x.ContractId == contractId && !x.Status.Equals("CHOOSE")).ToListAsync();
                        foreach (var n in medicalInstructionChooseIds)
                        {
                            miShares.Where(x => x.MedicalInstructionId == n).FirstOrDefault().Status = "CHOOSE";
                        }
                        await _repoMIShare.UpdateRange(miShares);
                    }
                    */
                    // Update DateStarted when doctor approve
                    if (dateStarted != null)
                    {
                        contract.DateStarted = dateStarted.GetValueOrDefault();
                    }
                    // Update DaysOfTracking when doctor approve
                    if (daysOfTracking != null)
                    {
                        contract.DaysOfTracking = daysOfTracking.GetValueOrDefault();
                        contract.DateFinished = contract.DateStarted.AddDays(daysOfTracking.GetValueOrDefault());
                        var license = await _repoLicense.GetDbSet().Where(x => x.Days >= contract.DaysOfTracking).OrderBy(x => x.Days).FirstOrDefaultAsync();
                        if (license != null)
                        {
                            contract.LicenseId = license.LicenseId;
                            contract.NameLicense = license.Name;
                            contract.PriceLicense = license.Price;
                        }
                        
                    }
                    // Update Status
                    if (!string.IsNullOrEmpty(status) && !contract.Status.Equals(status.ToUpper()))
                    {
                        contract.Status = status.ToUpper();
                    }

                    if (status.ToUpper().Equals("ACTIVE"))
                    {
                        var dateNow = DateTime.Now;
                        /*
                        // insert medicalInstruction from MedicalInstructionShare have status "CHOOSE"
                        var miToInsert = await _repoMIShare.GetDbSet().Where(x => x.ContractId == contractId &&
                        x.Status.Equals("CHOOSE")).Include(x => x.MedicalInstruction).Select(x => new MedicalInstruction()
                        {
                            DateCreate = dateNow,
                            Description = x.MedicalInstruction.Description,
                            Diagnose = x.MedicalInstruction.Diagnose,
                            MedicalInstructionImages = x.MedicalInstruction.MedicalInstructionImages,
                            MedicalInstructionTypeId = x.MedicalInstruction.MedicalInstructionTypeId,
                            PrescriptionId = x.MedicalInstruction.PrescriptionId,
                            Status = "CONTRACT",
                            MIShareFromId = x.MedicalInstructionId
                        }).ToListAsync();
                        // update Datecreate and status
                        // Create Healthrecord between Doctor and Patient when patient sign 
                        var healthRecord = new HealthRecord()
                        {
                            ContractId = contract.ContractId,
                            DateCreated = dateNow,
                            Diseases = contract.Diseases,
                            PersonalHealthRecordId = contract.Patient.PersonalHealthRecord.PersonalHealthRecordId,
                            Place = "Bác sĩ " + contract.FullNameDoctor,
                            Description = "Hồ sơ sức khỏe tại bác sĩ " + contract.FullNameDoctor,
                            MedicalInstructions = miToInsert
                        };
                        */
                        // Update stasus of HR
                        contract.HealthRecord.Status = "SIGNED";
                    }
                    // Status LOCKED
                    if (status.Equals("LOCKED"))
                    {
                        contract.DateLocked = DateTime.Now;
                        contract.ReasonLocked = "ActionFirstTime";
                    }
                    //Status APPROVED
                    if (status.Equals("APPROVED"))
                    {
                        var dateNow = DateTime.Now;
                        contract.DateApproved = dateNow;
                        // Create Healthrecord between Doctor and Patient when patient sign 
                        var healthRecord = new HealthRecord()
                        {
                            ContractId = contract.ContractId,
                            DateCreated = dateNow,
                            Diseases = contract.Diseases,
                            PersonalHealthRecordId = contract.Patient.PersonalHealthRecord.PersonalHealthRecordId,
                            Place = "Bác sĩ " + contract.FullNameDoctor,
                            Description = "Hồ sơ sức khỏe từ bác sĩ " + contract.FullNameDoctor,
                            Status = "UNSIGNED",
                            AppointmentFirst = false,
                            VitalSignScheduleFirst = false,
                            DateFinished = contract.DateFinished,
                            DateStarted = contract.DateStarted
                            /*
                            MedicalInstructions = contract.MedicalInstructions.Where(x => medicalInstructionChooseIds.Any(y => y == x.MedicalInstructionId)).Select(x => new MedicalInstruction()
                            {
                                DateCreate = dateNow,
                                Description = x.Description,
                                MedicalInstructionImages = x.MedicalInstructionImages,                                 Diagnose = x.Diagnose,
                                MedicalInstructionTypeId = x.MedicalInstructionTypeId,
                                MIShareFromId = x.MedicalInstructionId,
                                PrescriptionId = x.PrescriptionId,
                                VitalSignScheduleId = x.VitalSignScheduleId,
                                Status = "CONTRACT",                               
                            }).ToList()
                            */
                        };
                        if (medicalInstructionChooses.Any())
                        {
                            var mis = new List<MedicalInstruction>();
                            foreach (var mi in medicalInstructionChooses)
                            {
                                var tmp = await _repoMI.GetDbSet().Include(x => x.ContractMedicalInstructions).Where(x => x.MedicalInstructionId == mi).FirstOrDefaultAsync();
                                tmp.ContractMedicalInstructions = tmp.ContractMedicalInstructions.Where(x => x.ContractId == contractId).ToList();
                                if (tmp != null)
                                {
                                    var tmp2 = tmp.ContractMedicalInstructions.Where(x => x.ContractId == contractId).FirstOrDefault();
                                    if (tmp2 != null)
                                    {
                                        var medicalInstruction = new MedicalInstruction()
                                        {
                                            Conclusion = tmp2.Conclusion,
                                            MedicalInstructionImages = tmp.MedicalInstructionImages,
                                            MIShareFromId = tmp.MedicalInstructionId,
                                            Status = "CONTRACT",
                                            MedicalInstructionTypeId = tmp.MedicalInstructionTypeId,
                                            VitalSignScheduleId = tmp.VitalSignScheduleId,
                                            PrescriptionId = tmp.PrescriptionId,
                                            Description = tmp.Description,
                                            DateCreate = DateTime.Now,
                                        };
                                        var tmp3 = tmp2.DiseaseIds != null ? tmp2.DiseaseIds.Split(",")
                                            .Where(x => x != null && x.Trim().Length > 0).Select(x => x.Trim()) : null;
                                        if (tmp3 != null)
                                        {
                                            var diseases = await _repoDisease.GetDbSet().Where(x => tmp3.Any(y => y.Equals(x.DiseaseId))).ToListAsync();
                                            medicalInstruction.Diseases = diseases;
                                            mis.Add(medicalInstruction);
                                        }
                                    }
                                }
                            }
                            if (mis.Any())
                            {
                                healthRecord.MedicalInstructions = mis;
                            }
                            /*
                            var medicalInstructionIds = new List<int>();
                            var medicalInstructions = new List<MedicalInstruction>();
                            foreach (var miId in medicalInstructionDiseaseChooses)
                            {
                                medicalInstructionIds.AddRange(miId.MedicalInstructionIds);
                            }
                            // Loại bỏ phần tử trùng giữ lại 1. Distin
                            medicalInstructionIds = medicalInstructionIds.Distinct().ToList();
                            foreach (int id in medicalInstructionIds)
                            {
                                // Get MI ở DB
                                var mi = await _repoMI.GetDbSet().Where(x => x.MedicalInstructionId == id)
                                    .Select(x => new MedicalInstruction()
                                    {
                                        DateCreate = x.DateCreate,
                                        Description = x.Description,
                                        MedicalInstructionImages = x.MedicalInstructionImages,
                                        MedicalInstructionTypeId = x.MedicalInstructionTypeId,
                                        MIShareFromId = x.MedicalInstructionId,
                                        PrescriptionId = x.PrescriptionId,
                                        Diagnose = x.Diagnose,
                                        Status = "CONTRACT",
                                        VitalSignScheduleId = x.VitalSignScheduleId,
                                        DiseaseId = x.DiseaseId
                                        // Lấy các disease cuar MI
                                        })
                                    .FirstOrDefaultAsync();                                
                                // Insert vào list MI của HR
                                medicalInstructions.Add(mi);       
                                */
                        }
                        await _repoHR.Insert(healthRecord);
                    }
                    //Status SIGNED
                    if (status.Equals("SIGNED"))
                    {
                        var hr = await _repoHR.GetDbSet().Where(x => x.ContractId == contractId).FirstOrDefaultAsync();
                        if (hr != null)
                        {
                            hr.Status = "SIGNED";
                            await _repoHR.Update(hr);
                        }
                        contract.DateSigned = DateTime.Now;

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
                var contract = await _repo.GetDbSet().Where(x => x.ContractId == contractId).Include(x => x.Diseases).Include(x => x.Doctor)
                    .Include(x => x.Patient).ThenInclude(x => x.Account)
                    .Include(x => x.HealthRecord).ThenInclude(x => x.MedicalInstructions)
                    .ThenInclude(x => x.MedicalInstructionImages)
                    .Include(x => x.ContractMedicalInstructions).ThenInclude(x => x.MedicalInstruction.MedicalInstructionImages)
                    .Include(x => x.ContractMedicalInstructions).ThenInclude(x => x.MedicalInstruction.Diseases)
                    .Include(x => x.ContractMedicalInstructions).ThenInclude(x => x.MedicalInstruction.MedicalInstructionType)
                    .Include(x => x.HealthRecord.MedicalInstructions).ThenInclude(x => x.MedicalInstructionType)
                    .Include(x => x.HealthRecord.MedicalInstructions).ThenInclude(x => x.Diseases)
                    .FirstOrDefaultAsync();

                if (contract != null)
                {
                    var respone = new ContractDetailInformation()
                    {
                        ContractId = contractId.GetValueOrDefault(),
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
                        DiseaseContracts = contract.Diseases.Select(x => x.DiseaseId + ":" + x.Name).ToList(),
                        NameLicense = contract.NameLicense,
                        PriceLicense = contract.PriceLicense,
                    };

                    // MI have disease patient share
                    /*
                    MedicalInstructionDiseases = contract.MedicalInstructions.Where(x => x.Diseases.Any()).GroupBy(x => x.Disease)
                    .Select(x => new ContractDetailInformation.MedicalInstructionDisease() {
                        DiseaseId = x.Key.DiseaseId,
                        NameDisease = x.Key.Name,
                        MedicalInstructions= x.Select(y => new ContractDetailInformation.MedicalInstruction() {
                            MedicalInstructionId = y.MedicalInstructionId,
                            MedicalInstructionTypeName = y.MedicalInstructionType.Name,
                            Images = y.MedicalInstructionImages.Any() ? y.MedicalInstructionImages.Select(z => z.Image).ToList() : null,
                            Description = y.Description,
                            Conclusion = y.Conclusion
                        }).ToList()
                    }).ToList(),
                    */

                    if (contract.Diseases.Any())
                    {
                        var tmp = new List<ContractDetailInformation.MedicalInstructionDisease>();
                        // MI have Disease same HR
                        foreach (var disease in contract.Diseases)
                        {
                            var tmp2 = new ContractDetailInformation.MedicalInstructionDisease()
                            {
                                DiseaseId = disease.DiseaseId,
                                NameDisease = disease.Name,
                                MedicalInstructions =  contract.ContractMedicalInstructions.Where(x => x.DiseaseChoosedId != null ? x.DiseaseChoosedId.Equals(disease.DiseaseId) : false).Select(x => new ContractDetailInformation.MedicalInstruction()
                                {
                                    Conclusion = x.Conclusion,
                                    DateCreated = x.MedicalInstruction.DateCreate,
                                    Description = x.Description,
                                    MedicalInstructionId = x.MedicalInstruction.MedicalInstructionId,
                                    Images = x.MedicalInstruction.MedicalInstructionImages.Any() ? x.MedicalInstruction.MedicalInstructionImages.Select(z => z.Image).ToList() : null,
                                    MedicalInstructionTypeName = x.MedicalInstruction.MedicalInstructionType.Name,
                                    MIShareFromId = x.MedicalInstruction.MIShareFromId,
                                    Diseases = x.DiseaseIds != null ? x.DiseaseIds.Split(",").Where(x => x != null && x.Trim().Length > 0)
                                .Select(x =>{
                                    var tmp = _repoDisease.GetDbSet().Where(y => y.DiseaseId.Equals(x)).FirstOrDefault();
                                    return tmp.DiseaseId + "-"+tmp.Name;
                                    }).ToList() : null
                        }).ToList()
                            };
                            tmp.Add(tmp2);
                        }
                        respone.MedicalInstructionDiseases = tmp;
                    }
                    // MIs other patient share
                    var miOther = contract.ContractMedicalInstructions.Where(x => string.IsNullOrEmpty(x.DiseaseChoosedId)).ToList();
                    // Get full disease of MedicalInstruction
                    if (miOther.Any())
                    {
                        respone.MedicalInstructionOthers = new List<ContractDetailInformation.MedicalInstruction>();
                        foreach(var mi in miOther)
                        {
                            var tmp = mi.DiseaseIds.Split(",").Where(x => x != null && x.Trim().Length > 0)
                                .Select(x => x.Trim());
                            var tmp2 = _repoDisease.GetDbSet().Where(x => tmp.Any(y => y.Equals(x.DiseaseId)))
                                .Select(x => x.DiseaseId + "-" + x.Name).ToList();
                            var tmp3 = new ContractDetailInformation.MedicalInstruction()
                            {
                                MedicalInstructionId = mi.MedicalInstructionId,
                                MedicalInstructionTypeName = mi.MedicalInstruction.MedicalInstructionType.Name,
                                Images = mi.MedicalInstruction.MedicalInstructionImages.Any() ? mi.MedicalInstruction.MedicalInstructionImages.Select(x => x.Image).ToList() : null,
                                Description = mi.Description,
                                Conclusion = mi.Conclusion,
                                DateCreated = mi.MedicalInstruction.DateCreate,
                                Diseases = tmp2
                            };
                            respone.MedicalInstructionOthers.Add(tmp3);
                        }
                    }
                    // MI choosed 
                    if (contract.HealthRecord != null)
                    {
                        respone.MedicalInstructionChoosed = contract.HealthRecord.MedicalInstructions.Where(x => x.Status.Equals("CONTRACT")).Select(x => new ContractDetailInformation.MedicalInstruction()
                        {
                            MedicalInstructionId = x.MedicalInstructionId,
                            MedicalInstructionTypeName = x.MedicalInstructionType.Name,
                            Description = x.Description,
                            MIShareFromId = x.MIShareFromId,
                            Conclusion = x.Conclusion,
                            Images = x.MedicalInstructionImages != null ? x.MedicalInstructionImages.Select(y => y.Image).ToList() : null,
                            Diseases = x.Diseases.Any() ? x.Diseases.Select(x => x.DiseaseId + "-"+x.Name).ToList() : null
                        }).ToList();
                    }
                    return respone;
                }
            }
            return null;
        }

    public async Task<ICollection<Contract>> GetAllContractsByStatus(string status)
    {
        var contract = await _repo.GetDbSet().Include(x => x.Patient)
            .Include(x => x.HealthRecord).Where(x => !string.IsNullOrEmpty(status) ? x.Status.Equals(status) : true).Include(x => x.Doctor).ToListAsync();
        if (contract.Any())
        {
            return contract;
        }
        return null;
    }

    public async Task<ICollection<ContractInformation>> GetAllContractsByAdmin(string status)
    {
        var contracts = await _repo.GetDbSet().Where(x => (!string.IsNullOrEmpty(status) ? x.Status.Equals(status) : true)).Select(x => new ContractInformation()
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
            PatientId = x.PatientId,
            FullNameDoctor = x.Doctor.Account.FullName,
            PhoneNumberDoctor = x.Doctor.Account.PhoneNumber,
            //Disease
            Diseases = x.Diseases.Select(y => new ContractInformation.Disease()
            {
                DiseaseId = y.DiseaseId,
                Name = y.Name
            }).ToList(),
            Note = x.Note
        }).ToListAsync();
        if (contracts.Any())
        {
            return contracts;
        }
        return null;
    }

    public async Task<bool> UpdateContractToDemo(int contractId, string status, DateTime? TimeStarted)
    {
        if (contractId != 0)
        {
            var contract = await _repo.GetById(contractId);
            if (contract != null)
            {
                if (!string.IsNullOrEmpty(status))
                {
                    contract.Status = status;
                }
                if (TimeStarted != null)
                {
                    contract.DateStarted = TimeStarted.GetValueOrDefault().Date;
                }
                if (await _repo.Update(contract))
                {
                    await _uow.CommitAsync();
                    return true;
                }
            }
        }
        return false;
    }

}
}
