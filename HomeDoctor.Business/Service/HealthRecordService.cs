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
        private readonly IRepositoryBase<MedicalInstructionShare> _repoMiShare;
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IUnitOfWork _uow;

        public HealthRecordService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<HealthRecord>();
            _repoPHR = _uow.GetRepository<PersonalHealthRecord>();
            _repoDisease = _uow.GetRepository<Disease>();
            _repoMiShare = _uow.GetRepository<MedicalInstructionShare>();
            _repoMI = _uow.GetRepository<MedicalInstruction>();
        }

        public async Task<bool> CreateHealthRecord(HealthRecordCreate healthRecord)
        {
           if(healthRecord != null)
            {               
                var patient = _repoPHR.GetDbSet().Where(x => x.PatientId == healthRecord.PatientId).FirstOrDefault();
                var tmp = new HealthRecord()
                {
                    PersonalHealthRecordId = patient.PersonalHealthRecordId,
                    DateCreated = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7),
                    Description = healthRecord.Description,
                    Place = healthRecord.Place,
                };
                // Get dieases to create healthRecord 
                if (healthRecord.DiceaseIds != null)
                {
                    tmp.Diseases = new List<Disease>();
                    foreach(var diceaseId in healthRecord.DiceaseIds)
                    {
                        var disease = await _repoDisease.GetById(diceaseId);
                        tmp.Diseases.Add(disease);
                    }
                }
                var check = await _repo.Insert(tmp);
                if (check)
                {
                    await _uow.CommitAsync();
                    return true;
                }
            }
            return false;
        }      

        public async Task<HealthRecordInformation> GetHealthRecordById(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var hr = await _repo.GetDbSet().Where(x => x.HealthRecordId == healthRecordId).Include(x => x.Diseases).FirstOrDefaultAsync();
                if(hr != null)
                {
                    var tmp = new HealthRecordInformation()
                    {
                        Diseases = hr.Diseases.Select(x => new HealthRecordInformation.Disease() {
                            DiseaseId = x.DiseaseId,
                            DiseaseName = x.Name
                        }).ToList(),
                        DateCreated = hr.DateCreated,
                        Description = hr.Description,
                        ContractId = hr.ContractId,
                        Place = hr.Place
                    };
                    return tmp;
                }
            }

            return null;
        }

        public async Task<ICollection<HealthRecordInformation>> GetHealthRecordByPatientId(int patientId, bool? onSystem)
        {
           if(patientId != 0)
            {
                var healthRecords = await _repo.GetDbSet().Where(x => x.PersonalHealthRecord.PatientId == patientId && (onSystem == true ? x.ContractId != null: true)).Include(x => x.Diseases).Select(x => new HealthRecordInformation()
                {
                    HealthRecordId = x.HealthRecordId,
                    Diseases  = x.Diseases.Select(y => new HealthRecordInformation.Disease() { 
                        DiseaseId = y.DiseaseId,
                        DiseaseName = y.Name
                    }).ToList(),
                    Place = x.Place,
                    Description = x.Description,
                    DateCreated = x.DateCreated,
                    ContractId = x.Contract.ContractId
                }).ToListAsync();
                if(healthRecords.Count != 0)
                {
                    return healthRecords;
                }
            }
            return null;
        }
        // Doctor manage HealthRecord of Patient

        public async Task<HealthRecordOverviewRespone> GetHROverviewByHRId(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var overviewHr = await _repo.GetDbSet().Where(x => x.HealthRecordId == healthRecordId).Include(x => x.Contract).Include(x => x.PersonalHealthRecord).ThenInclude(x => x.Patient).ThenInclude(x => x.Account).Include(x => x.Diseases).Select(x => new HealthRecordOverviewRespone()
                {
                    FullNamePatient = x.Contract.FullNamePatient,
                    AddressPatient = x.Contract.AddressPatient,
                    PhoneNumberPatient = x.Contract.PhoneNumberPatient,
                    DOBPatient = x.Contract.DOBPatient,
                    Gender = x.PersonalHealthRecord.Patient.Account.Gender,
                    Career = x.PersonalHealthRecord.Patient.Career,
                    Height = x.PersonalHealthRecord.Patient.Height,
                    Weight = x.PersonalHealthRecord.Patient.Weight,
                    PersonalMedicalHistory = x.PersonalHealthRecord.PersonalMedicalHistory,
                    FamilyMedicalHistory = x.PersonalHealthRecord.FamilyMedicalHistory,
                    AccountPatientId = x.PersonalHealthRecord.Patient.AccountId,
                    Diseases = x.Diseases.Select(y => new HealthRecordOverviewRespone.Disease()
                    {
                        DiseaseId = y.DiseaseId,
                        DiseaseName = y.Name
                    }).ToList()
                }).FirstOrDefaultAsync();
                if(overviewHr != null)
                {
                    return overviewHr;
                }
            }
            return null;
        }
        public async Task<HealingConditions> GetHealingConditions(int healthRecordId, int contractId)
        {
            if (healthRecordId != 0 && contractId != 0)
            {
                // Check connect with smartWatch ? and Get medicalInstruction created or inserted
                var connectedSW = await _repoPHR.GetDbSet().Where(x => x.HealthRecords.Any(y => y.HealthRecordId == healthRecordId)).Select(x => x.SmartWatchConnected).FirstOrDefaultAsync();
                // Get medicalInstruction shared
                var miShare = await _repoMiShare.GetDbSet().Where(x => x.ContractId == contractId && x.Status.Equals("CONTRACT")).Include(x => x.MedicalInstruction.MedicalInstructionType)
                    .Select(x => x.MedicalInstruction).ToListAsync();
                /*
                // Get MedicalInstruoc Created Or Insert on HR
                var mi = await _repoMI.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && (x.MedicalInstructionTypeId == 4 || x.MedicalInstructionTypeId == 6)).Include(x => x.MedicalInstructionType).ToListAsync();
                */
                
                var respone = new HealingConditions()
                {
                    SmartWatchConnected = connectedSW,
                    MedicalInstructionTypes = miShare.GroupBy(x => x.MedicalInstructionType.Name).Select(x => new HealingConditions.MedicalInstructionType()
                    {
                        MIType = x.Key,
                        MedicalInstructions = x.Select(y => new HealingConditions.MedicalInstructionShare()
                        {
                            MedicalInstructionId = y.MedicalInstructionId,
                            Image = y.Image,
                            Diagnose = y.Diagnose
                        }).ToList()
                    }).ToList()
                };
                if(respone != null)
                {
                    return respone;
                }                
            }
            return null;
        }
    }
}
