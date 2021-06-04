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
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IRepositoryBase<HealthRecord> _repo;
        private readonly IRepositoryBase<PersonalHealthRecord> _repoPHR;
        private readonly IRepositoryBase<Disease> _repoDisease;
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IUnitOfWork _uow;

        public HealthRecordService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<HealthRecord>();
            _repoPHR = _uow.GetRepository<PersonalHealthRecord>();
            _repoDisease = _uow.GetRepository<Disease>();
            _repoMI = _uow.GetRepository<MedicalInstruction>();
        }

        public async Task<int> CreateHealthRecord(HealthRecordCreate healthRecord)
        {
            if (healthRecord != null)
            {
                var patient = _repoPHR.GetDbSet().Where(x => x.PatientId == healthRecord.PatientId).FirstOrDefault();
                var tmp = new HealthRecord()
                {
                    PersonalHealthRecordId = patient.PersonalHealthRecordId,
                    DateCreated = DateTime.Now,
                    Description = healthRecord.Description,
                    Place = healthRecord.Place,
                    DateStarted = healthRecord.DateStarted,
                    DateFinished = healthRecord.DateFinished,                   
                };
                // Get dieases to create healthRecord 
                if (healthRecord.DiseaseIds != null)
                {
                    tmp.Diseases = new List<Disease>();
                    foreach (var diceaseId in healthRecord.DiseaseIds)
                    {
                        var disease = await _repoDisease.GetById(diceaseId);
                        tmp.Diseases.Add(disease);
                    }
                }
                var check = await _repo.Insert(tmp);
                if (check)
                {
                    await _uow.CommitAsync();
                    return tmp.HealthRecordId;
                }
            }
            return 0;
        }

        public async Task<bool> DeleteHealthRecord(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var hr = await _repo.GetDbSet().Include(x => x.MedicalInstructions).Where(x => x.HealthRecordId == healthRecordId).FirstOrDefaultAsync();
                if(hr != null)
                {
                    hr.Status = "DELETE";
                    if (hr.MedicalInstructions.Any())
                    {
                        foreach(var mi in hr.MedicalInstructions)
                        {
                            mi.Status = "DELETE";
                        }
                    }
                    if(await _repo.Update(hr))
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<HealthRecordInformation> GetHealthRecordById(int healthRecordId)
        {
            if (healthRecordId != 0)
            {
                var hr = await _repo.GetDbSet().Where(x => x.HealthRecordId == healthRecordId)
                    .Include(x => x.Diseases).Include(x => x.Contract).FirstOrDefaultAsync();
                if (hr != null)
                {
                    var tmp = new HealthRecordInformation()
                    {
                        ContractStatus = hr.ContractId != null ? hr.Contract.Status : null,
                        HealthRecordId = hr.HealthRecordId,                        
                        Diseases = hr.Diseases.Select(x => new HealthRecordInformation.Disease()
                        {
                            DiseaseId = x.DiseaseId,
                            DiseaseName = x.Name
                        }).ToList(),
                        DateCreated = hr.DateCreated,
                        Description = hr.Description,
                        ContractId = hr.ContractId,
                        Place = hr.Place,
                        DateFinished = hr.DateFinished,
                        DateStarted = hr.DateStarted,
                        Status = hr.Status
                    };
                    return tmp;
                }
            }

            return null;
        }

        public async Task<ICollection<HealthRecordInformation>> GetHealthRecordByPatientId(int patientId, bool? onSystem)
        {
            if (patientId != 0)
            {
                var healthRecords = await _repo.GetDbSet().Where(x => x.PersonalHealthRecord.PatientId == patientId && (onSystem == true ? x.ContractId != null : true) && (onSystem == false ? x.ContractId == null : true) && !x.Status.Equals("DELETE") && !x.Status.Equals("UNSIGNED") && (x.ContractId != null ? x.Contract.Status.Equals("ACTIVE")|| x.Contract.Status.Equals("FINISHED") : true) ).Include(x => x.Diseases).Select(x => new HealthRecordInformation()
                {
                    HealthRecordId = x.HealthRecordId,
                    Diseases = x.Diseases.Select(y => new HealthRecordInformation.Disease()
                    {
                        DiseaseId = y.DiseaseId,
                        DiseaseName = y.Name
                    }).ToList(),
                    Place = x.Place,
                    Description = x.Description,
                    DateCreated = x.DateCreated,
                    ContractId = x.Contract.ContractId,
                    DateFinished = (x.ContractId != null ? x.Contract.DateFinished : x.DateFinished),
                    Status = x.Status,
                    ContractStatus = x.Contract.Status,
                    DateStarted = (x.ContractId != null ? x.Contract.DateStarted : x.DateStarted),                   
                }).ToListAsync();
                if (healthRecords.Any())
                {
                    return healthRecords;
                }
            }
            return null;
        }
        // Doctor manage HealthRecord of Patient

        public async Task<HealthRecordOverviewRespone> GetHROverviewByHRId(int healthRecordId)
        {
            if (healthRecordId != 0)
            {
                var overviewHr = await _repo.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && x.ContractId != null)
                    .Include(x => x.Contract).Include(x => x.PersonalHealthRecord).ThenInclude(x => x.Patient).ThenInclude(x => x.Account).Include(x => x.Diseases).Include(x => x.Contract)
                    .Include(x => x.MedicalInstructions).ThenInclude(x => x.MedicalInstructionImages)
                    .Include(x => x.MedicalInstructions).ThenInclude(x => x.MedicalInstructionType)
                    .Include(x => x.Appointments)
                    .FirstOrDefaultAsync();
                if (overviewHr != null)
                {
                    var respone = new HealthRecordOverviewRespone()
                    {
                        FullNamePatient = overviewHr.Contract.FullNamePatient,
                        AddressPatient = overviewHr.Contract.AddressPatient,
                        PhoneNumberPatient = overviewHr.Contract.PhoneNumberPatient,                        
                        DOBPatient = overviewHr.Contract.DOBPatient,
                        Gender = overviewHr.PersonalHealthRecord.Patient.Account.Gender,
                        Career = overviewHr.PersonalHealthRecord.Patient.Career,
                        Height = overviewHr.PersonalHealthRecord.Patient.Height,
                        Weight = overviewHr.PersonalHealthRecord.Patient.Weight,
                        PersonalMedicalHistory = overviewHr.PersonalHealthRecord.PersonalMedicalHistory,
                        FamilyMedicalHistory = overviewHr.PersonalHealthRecord.FamilyMedicalHistory,
                        AccountPatientId = overviewHr.PersonalHealthRecord.Patient.AccountId,
                        SmartWatchConnected = overviewHr.PersonalHealthRecord.SmartWatchConnected,
                        Diseases = overviewHr.Diseases.Select(y => new HealthRecordOverviewRespone.Disease()
                        {
                            DiseaseId = y.DiseaseId,
                            DiseaseName = y.Name
                        }).ToList(),
                        ContractDetail = new HealthRecordOverviewRespone.Contract()
                        {
                            ContractId = overviewHr.ContractId.GetValueOrDefault(),
                            DateStarted = overviewHr.Contract.DateStarted.ToString("dd/MM/yyyy"),
                            DateFinished = overviewHr.Contract.DateFinished.ToString("dd/MM/yyyy")
                        },
                        MedicalInstructions = overviewHr.MedicalInstructions.Any() ? overviewHr.MedicalInstructions.Where(y => y.Status.Equals("CONTRACT"))
                    .Select(x => new HealthRecordOverviewRespone.MedicalInstruction()
                    {
                        MedicalInstructionId = x.MedicalInstructionId,
                        Images = x.MedicalInstructionImages.Any() ? x.MedicalInstructionImages.Select(x => x.Image).ToList() : null,
                        Conclusion = x.Conclusion,
                        MedicalInstructionType = x.MedicalInstructionType.Name,
                        PrescriptionId = x.PrescriptionId,
                        VitalSignScheduleId = x.VitalSignScheduleId
                    }).ToList() : null,
                        AppointmentNext = overviewHr.Appointments != null ? overviewHr.Appointments
                    .Where(y => y.Status.Equals("ACTIVE")).OrderByDescending(y => y.DateExamination).Select(y => new HealthRecordOverviewRespone.Appointment()
                    {
                        AppointmentId = y.AppointmentId,
                        DateExamination = y.DateExamination,
                        Note = y.Note,
                        Status = y.Status
                    }).FirstOrDefault() : null
                    };
                    return respone;
                }
            }
            return null;
        }

        public async Task<bool> UpdateActionFirstTimeToDemo(int healthRecordId, bool actionFirstTime)
        {
            if (healthRecordId != 0)
            {
                var hr = await _repo.GetById(healthRecordId);
                if (hr != null)
                {
                    hr.AppointmentFirst = actionFirstTime;
                    hr.VitalSignScheduleFirst = actionFirstTime;
                    if (await _repo.Update(hr))
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> UpdateHealthRecord(int healthRecordId, string place, string description, ICollection<string> diseases)
        {
            if (healthRecordId != 0)
            {
                var hr = await _repo.GetDbSet().Include(x => x.Diseases)
                    .Where(x => x.HealthRecordId == healthRecordId).FirstOrDefaultAsync();
                if (hr != null)
                {
                    if (hr.ContractId != null)
                    {
                        if (!string.IsNullOrEmpty(description))
                        {
                            hr.Description = description;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(description))
                        {
                            hr.Description = description;
                        }
                        if (!string.IsNullOrEmpty(place))
                        {
                            hr.Place = place;
                        }
                        if (diseases.Any())
                        {
                            hr.Diseases = await _repoDisease.GetDbSet().Where(x => diseases.Any(y => y.Equals(x.DiseaseId))).ToListAsync();
                        }
                    }
                    if (await _repo.Update(hr))
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
