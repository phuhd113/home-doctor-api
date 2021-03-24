using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class MedicalInstructionService : IMedicalInstructionService
    {
        private IRepositoryBase<Data.Models.MedicalInstruction> _repoMI;
        private IRepositoryBase<HealthRecord> _repoHR;
        private IMedicalInstructionTypeService _serMIT;
        private IUnitOfWork _uow;

        public MedicalInstructionService(IUnitOfWork uow,IMedicalInstructionTypeService serMIT)
        {
            _uow = uow;
            _repoMI = _uow.GetRepository<Data.Models.MedicalInstruction>();
            _serMIT = serMIT;
            _repoHR = _uow.GetRepository<HealthRecord>();
        }

        public async Task<bool> CreateMedicalInstructionWithImage(ViewModel.RequestModel.MedicalInstructionCreate medicalInstruction, string pathImage)
        {
            if(medicalInstruction != null)
            {
                Data.Models.MedicalInstruction mi = new Data.Models.MedicalInstruction()
                {
                    DateCreate = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7),
                    Description = medicalInstruction.Description,
                    Diagnose = medicalInstruction.Diagnose,
                    HealthRecordId = medicalInstruction.HealthRecordId,
                    Image = pathImage,
                    MedicalInstructionTypeId = medicalInstruction.MedicalInstructionTypeId,                  
                };
                var check = await _repoMI.Insert(mi);
                if (check)
                {
                    await _uow.CommitAsync();
                    return true;
                }
            }
           return false;
        }
        public async Task<MedicalInstructionInformation> GetMedicalInstructionById(int id)
        {
            if(id != 0)
            {
                var mi = await _repoMI.GetDbSet().Where(x => x.MedicalInstructionId == id).Include(x => x.HealthRecord).ThenInclude(x => x.Contract).Include(x => x.Prescription).ThenInclude(x => x.MedicationSchedules).Include(x => x.MedicalInstructionType).FirstOrDefaultAsync();
                if(mi != null)
                {
                    var tmp = new MedicalInstructionInformation()
                    {
                        MedicalInstructionId = id,
                        PatientFullName = mi.HealthRecord.Contract.FullNamePatient,
                        PlaceHealthRecord = mi.HealthRecord.Place,
                        MedicalInstructionType = mi.MedicalInstructionType.Name,
                        Description = mi.Description,
                        Diagnose = mi.Diagnose,                      
                    };
                    if(mi.Image != null)
                    {
                        tmp.Image = mi.Image;
                    }
                    else
                    {
                        if(mi.MedicalInstructionTypeId == 1)
                        {
                            tmp.PrescriptionRespone = new PrescriptionRespone()
                            {
                                DateStarted = mi.Prescription.DateStarted,
                                DateFinished = mi.Prescription.DateFinished,
                                DateCanceled = mi.Prescription.DateCanceled,
                                ReasonCancel = mi.Prescription.ReasonCancel,
                                Status = mi.Prescription.Status,                               
                                MedicationSchedules = mi.Prescription.MedicationSchedules.Select(x => new MedicationScheduleRespone()
                                {
                                    MedicationName = x.MedicationName,
                                    Content = x.Content,
                                    Unit = x.Unit,
                                    UseTime = x.UseTime,
                                    Morning = x.Morning,
                                    Noon = x.Noon,
                                    Night = x.Night,
                                    AfterNoon = x.AfterNoon
                                }).ToList()
                            };
                        }                        
                    }
                    return tmp;
                }
            }
            return null;
        }

        public async Task<ICollection<MedicalInstructionByDiseaseRespone>> GetMIToCreateContract(int patientId,ICollection<string> diseaseIds,int medicalInstructionType)
        {
            if (patientId != 0)
            {
                var mis = await _repoMI.GetDbSet().Include(x => x.HealthRecord).ThenInclude(x => x.Diseases).Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId && x.HealthRecord.Diseases.Any(y => diseaseIds.Any(z => z.Equals(y.DiseaseId))
                ) && x.MedicalInstructionTypeId == medicalInstructionType).ToListAsync();
                if(mis.Any())
                {
                    var respone = new List<MedicalInstructionByDiseaseRespone>();
                    foreach(var tmp in mis.GroupBy(x => x.HealthRecord))
                    {
                        var entity = new MedicalInstructionByDiseaseRespone()
                        {
                            HealthRecordPlace = tmp.Key.Place,
                            DateCreate = tmp.Key.DateCreated.ToString("dd/MM/yyyy"),
                            MedicalInstructions = tmp.Select(x => new MedicalInstructionByDiseaseRespone.MedicalInstruction()
                            {
                                MedicalInstructionId = x.MedicalInstructionId,
                                Image = x.Image,
                                DateCreate = x.DateCreate.ToString("dd/MM/yyyy")
                            }).OrderBy(x => x.DateCreate).ToList()
                        };
                        respone.Add(entity);
                    }
                return respone;                   
                }
                /*
                // Get medicalInstrucs from HealthRecord with patientId same disease
                var hrs = _repoHR.GetDbSet().Where(x => x.PersonalHealthRecord.PatientId == patientId && x.Diseases.Any(y => diseaseIds.Any(z => z.Equals(y.DiseaseId))) && x.MedicalInstructions.Count != 0).Include(x => x.MedicalInstructions).ThenInclude(x => x.MedicalInstructionType).Select(x => new
                {
                    x.Place,
                    x.Diseases,
                    x.MedicalInstructions
                }).ToList();
                if (hrs.Count != 0)
                {
                    var respone = new List<MedicalInstructionByDiseaseRespone>();
                    foreach (var tmp in hrs)
                    {
                        // Get by Type 4,6,9
                        var miTypes = tmp.MedicalInstructions.Where(x => x.MedicalInstructionTypeId == 6 || x.MedicalInstructionTypeId == 4 || x.MedicalInstructionTypeId == 9).GroupBy(x => x.MedicalInstructionType.Name);

                        if (miTypes.ToList().Count != 0)
                        {
                            foreach (var mi in miTypes)
                            {
                                var tmp1 = new MedicalInstructionByDiseaseRespone
                                {
                                    MedicalInstructionType = mi.Key,
                                    HealthRecords = mi.Select(x => new MedicalInstructionByDiseaseRespone.HealthRecord()
                                    {
                                        HealthRecordPlace = tmp.Place,
                                        MedicalInstructions = mi.Select(y => new MedicalInstructionByDiseaseRespone.MedicalInstruction()
                                        {
                                            MedicalInstructionId = y.MedicalInstructionId,
                                            Image = y.Image
                                        }).ToList()
                                    }).ToList()
                                };
                                respone.Add(tmp1);
                            }
                        }
                    }
                    return respone;
                }
                */
            }
            return null;
        }

        public async Task<ICollection<MedicalInstructionInformation>> GetMedicalInstructionsByHRId(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var hrs = await _repoMI.GetDbSet().Where(x => x.HealthRecordId == healthRecordId).Include(x => x.HealthRecord).OrderByDescending(x => x.DateCreate).Select(x => new MedicalInstructionInformation()
                {
                    MedicalInstructionId = x.MedicalInstructionId,
                    MedicalInstructionType = x.MedicalInstructionType.Name,
                    DateCreate = x.DateCreate,
                    Image = x.Image,
                    Diagnose = x.Diagnose,
                    Description = x.Description,
                    PlaceHealthRecord = x.HealthRecord.Place,                
                    PrescriptionRespone = x.Prescription != null ? new PrescriptionRespone()
                    {
                        DateStarted = x.Prescription.DateStarted,
                        DateFinished = x.Prescription.DateFinished,
                        ReasonCancel = x.Prescription.ReasonCancel,
                        DateCanceled = x.Prescription.DateCanceled,
                        Status = x.Prescription.Status,
                        MedicationSchedules = x.Prescription.MedicationSchedules.Select(y => 
                        new MedicationScheduleRespone() {
                            MedicationName = y.MedicationName,
                            Content = y.Content,
                            Unit = y.Unit,
                            UseTime = y.UseTime,
                            Morning = y.Morning,
                            Noon = y.Noon,
                            Night = y.Night,
                            AfterNoon = y.AfterNoon
                        }).ToList()
                    } : null
                }).ToListAsync();
                if(hrs.Count != 0)
                {
                    return hrs;
                }
            }
            return null;
        }

        public async Task<ICollection<MedicalInstructionToShareRespone>> GetMedicalInstructionToShare(int patientId, int contractId)
        {
            if(patientId != 0 && contractId != 0)
            {
                var mis = await _repoMI.GetDbSet().Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId && !x.MedicalInstructionShares.Any(y => y.ContractId == contractId)).Include(x => x.HealthRecord).Include(x => x.MedicalInstructionType).ToListAsync();
                var respone  = mis.GroupBy(x => x.HealthRecord.Place) // Group by HealthRecordPlace
                    .Select(x => new MedicalInstructionToShareRespone()
                    {
                        HealthRecordPlace = x.Key,
                        MedicalInstructionTypes = x.GroupBy(y => y.MedicalInstructionType.Name) // Group by type
                        .Select(z => new MedicalInstructionToShareRespone.MedicalInstructionType()
                        {
                            MIType = z.Key,
                            MedicalInstructions = z.Select(m => new MedicalInstructionToShareRespone.MedicalInstruction()
                            {
                                MedicalInstructionId = m.MedicalInstructionId,
                                Image = m.Image
                            }).ToList()
                        }).ToList()
                    }).ToList();
                if(respone.Any())
                {
                    return respone;
                }
            }
            return null;
        }

        // Prescription Service
        public async Task<ICollection<MedicalInstructionInformation>> GetPrescriptionByPatientId(int patientId, int? healthRecordId)
        {
            if(patientId != 0)
            {
                var tmp = await _repoMI.GetDbSet().Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId && x.MedicalInstructionTypeId == 1 && x.HealthRecord.Contract.Status.Equals("ACTIVE") && (healthRecordId != null ? x.HealthRecordId == healthRecordId : healthRecordId == null)).OrderByDescending(x => x.DateCreate).Select(x => new MedicalInstructionInformation()
                {
                    MedicalInstructionId = x.MedicalInstructionId,
                    MedicalInstructionType = x.MedicalInstructionType.Name,
                    Diagnose = x.Diagnose,                    
                    Description = x.Description,
                    PrescriptionRespone = new PrescriptionRespone()
                    {
                        DateStarted = x.Prescription.DateStarted,
                        DateFinished = x.Prescription.DateFinished,
                        DateCanceled = x.Prescription.DateCanceled,
                        Status = x.Prescription.Status,
                        ReasonCancel = x.Prescription.ReasonCancel,
                        MedicationSchedules = x.Prescription.MedicationSchedules.Select(y => new MedicationScheduleRespone() {
                            MedicationName = y.MedicationName,
                            Content = y.Content,
                            Unit = y.Unit,
                            UseTime = y.UseTime,
                            Morning = y.Morning,
                            Noon = y.Noon,
                            Night = y.Night,
                            AfterNoon = y.AfterNoon
                        }).ToList()
                    }                  
                }).ToListAsync();
                if(tmp.Count != 0)
                {
                    return tmp;
                }
            }
            return null;
        }

        public async Task<int> UpdatePrecription(int medicalInstructionId, string status, string? reasonCancel, ICollection<MIPresciption.MedicationSchedule> medicationSchedules)
        {
            if(medicalInstructionId != 0)
            {
                var mi = await _repoMI.GetDbSet().Where(x => x.MedicalInstructionId == medicalInstructionId).Include(x => x.Prescription.MedicationSchedules).FirstOrDefaultAsync();
                if(mi != null)
                {
                    if (status != null)
                    {
                        mi.Prescription.Status = status.ToUpper();
                        mi.Prescription.DateCanceled = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7);
                        if(reasonCancel != null)
                        {
                            mi.Prescription.ReasonCancel = reasonCancel;
                        }                        
                    }
                    else
                    {
                        if (medicationSchedules != null)
                        {
                            mi.Prescription.MedicationSchedules = medicationSchedules.Select(x => new MedicationSchedule()
                            {
                                MedicationName = x.MedicationName,
                                Content = x.Content,
                                Unit = x.Unit,
                                UseTime = x.UseTime,
                                Morning = x.Morning,
                                Noon = x.Noon,
                                AfterNoon = x.AfterNoon,
                                Night = x.Night
                            }).ToList();
                        }
                    }
                    var check = await _repoMI.Update(mi);
                    if (check)
                    {
                        await _uow.CommitAsync();
                        return mi.MedicalInstructionId;
                    }
                }                               
            }
            return 0;
        }

        public async Task<int> CreatePrescription(MIPresciption request)
        {
            if (request != null)
            {
                if (request.HealthRecordId != 0)
                {
                    // create list medicationSchedule to insert with MIId
                    var medicationSchedules = request.MedicationScheduleCreates.Select(x => new MedicationSchedule()
                    {
                        MedicationName = x.MedicationName,
                        Content = x.Content,
                        Unit = x.Unit,
                        UseTime = x.UseTime,
                        Morning = x.Morning,
                        AfterNoon = x.AfterNoon,
                        Night = x.Night,
                        Noon = x.Noon
                    }).ToList();
                    var prescription = new Prescription()
                    {
                        DateStarted = request.DateStart.Value,
                        DateFinished = request.DateFinish.Value,
                        MedicationSchedules = medicationSchedules,
                        Status = "ACTIVE"
                    };
                    var medicalInstruction = new MedicalInstruction()
                    {
                        HealthRecordId = request.HealthRecordId,
                        DateCreate = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7),
                        MedicalInstructionTypeId = 1,
                        Description = request.Description,
                        Diagnose = request.Diagnose,
                        Prescription = prescription
                    };
                    
                    var check = await _repoMI.Insert(medicalInstruction);
                    if (check)
                    {
                        await _uow.CommitAsync();
                        return medicalInstruction.MedicalInstructionId;
                    }
                }
            }
            return 0;
        }

        public async Task<int> CreateVitalSignSchedule(MIVitalSignSchedule request)
        {
            if(request.VitalSigns.Any())
            {
                // Create vitalSignType 
                var vitalSigns = request.VitalSigns.Select(x => new Data.Models.VitalSign()
                {
                    NumberMax = x.NumberMax,
                    NumberMin = x.NumberMin,
                    MinuteDangerInterval = x.MinuteDangerInterval,
                    TimeStart = x.TimeStart,
                    MinuteAgain = x.MinuteAgain,
                    VitalSignTypeId = x.VitalSignTypeId,                   
                }).ToList();
                //Create medicalInstruction and vitalsignSchedule
                var medicalInstruction = new MedicalInstruction()
                {
                    HealthRecordId = request.HealthRecordId,
                    DateCreate = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7),
                    MedicalInstructionTypeId = 1,
                    Description = request.Description,
                    Diagnose = request.Diagnose,                    
                    VitalSignSchedule = new VitalSignSchedule()
                    {
                        DateStarted = request.DateStart.Value,
                        DateFinished = request.DateFinish.Value,
                        Status = "ACTIVE",
                        VitalSigns = vitalSigns
                    }
                };
                if(await _repoMI.Insert(medicalInstruction))
                {
                    await _uow.CommitAsync();
                    return medicalInstruction.MedicalInstructionId;
                }               
            }
            return 0;
        }

        /*
        public async Task<bool> CheckDiseaseExist(string diseaseId, Disease disease)
        {
            if(diseaseId != null && disease != null)
            {
                var number = Int32.Parse(diseaseId.Substring(1));
                if (diseaseId.StartsWith(disease.Code))
                {
                    if(disease.Number == null)
                    {
                        if(number <= disease.End && number >= disease.Start)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if(number == disease.Number)
                        {
                            return true;
                        }
                    }
                }           
            }
            return false;
        }
        */
    }
}
