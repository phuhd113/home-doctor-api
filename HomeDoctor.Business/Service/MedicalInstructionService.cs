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
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IRepositoryBase<VitalSignSchedule> _repoVSSchedule;
        private readonly IRepositoryBase<Disease> _repoDisease;
        private readonly IRepositoryBase<HealthRecord> _repoHR;

        private readonly IUnitOfWork _uow;

        public MedicalInstructionService(IUnitOfWork uow)
        {
            _uow = uow;
            _repoMI = _uow.GetRepository<MedicalInstruction>();
            _repoVSSchedule = _uow.GetRepository<VitalSignSchedule>();
            _repoDisease = _uow.GetRepository<Disease>();
            _repoHR = _uow.GetRepository<HealthRecord>();
            
        }

        public async Task<bool> CreateMedicalInstructionWithImage(MedicalInstructionCreate medicalInstruction, ICollection<string> pathImages,string fromBy)
        {
            if(medicalInstruction != null)
            {
                MedicalInstruction mi = new MedicalInstruction()
                {
                    DateCreate = DateTime.Now,
                    Description = medicalInstruction.Description,
                    Conclusion = medicalInstruction.Conclusion,
                    HealthRecordId = medicalInstruction.HealthRecordId,
                    MedicalInstructionImages = pathImages.Any() ? pathImages.Select(x => new MedicalInstructionImage()
                    {
                        Image = x
                    }).ToList() : null,
                    MedicalInstructionTypeId = medicalInstruction.MedicalInstructionTypeId,
                    DateTreatment = medicalInstruction.DateTreatment.GetValueOrDefault()
                };
                // Get Disiease of medicalInstruction patient choose to insert 
                if (medicalInstruction.DiseaseIds != null)
                {
                    mi.Diseases = new List<Disease>();
                    var dict = _repoDisease.GetDbSet().ToDictionary(x => x.DiseaseId);
                    foreach (var diseaseId in medicalInstruction.DiseaseIds)
                    {
                        mi.Diseases.Add(dict[diseaseId]);
                    }
                    // Insert disease for HR if it's new
                    var hr = await _repoHR.GetDbSet().Include(x => x.Diseases).Where(x => x.HealthRecordId == medicalInstruction.HealthRecordId).FirstOrDefaultAsync();
                    if(hr != null)
                    {
                        var tmp = mi.Diseases.Except(hr.Diseases).ToList();
                        if (tmp.Any())
                        {
                            tmp.ForEach(x => hr.Diseases.Add(x));
                            await _repoHR.Update(hr);
                        }
                    }
                }               
                if (!string.IsNullOrEmpty(fromBy))
                {
                    if (fromBy.ToUpper().Equals("PATIENT"))
                    {
                        // if patient insert MI have contract
                        var hr = await _repoHR.GetById(medicalInstruction.HealthRecordId);
                        if (hr.ContractId != null)
                        {
                            mi.Status = "PENDING";
                        }
                        else
                        {
                            // if patient insert MI by patient create
                            mi.Status = "PATIENT";
                        }
                    }
                    else
                    {
                        if (fromBy.ToUpper().Equals("DOCTOR"))
                        {
                            mi.Status = "DOCTOR";
                        }
                    }
                }                   
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
                var mi = await _repoMI.GetDbSet().Where(x => x.MedicalInstructionId == id)
                    .Include(x => x.HealthRecord).ThenInclude(x => x.Contract)
                    .Include(x => x.HealthRecord.PersonalHealthRecord.Patient.Account)
                    .Include(x => x.Prescription).ThenInclude(x => x.MedicationSchedules)
                    .Include(x => x.VitalSignSchedule).ThenInclude(x => x.VitalSigns).ThenInclude(x => x.VitalSignType)
                    .Include(x => x.MedicalInstructionType)
                    .Include(x => x.MedicalInstructionImages)
                    .Include(x => x.Diseases)
                    .FirstOrDefaultAsync();
                if(mi != null)
                {
                    var tmp = new MedicalInstructionInformation()
                    {
                        MedicalInstructionId = id,
                        PatientFullName = mi.HealthRecord.PersonalHealthRecord.Patient.Account.FullName,
                        PlaceHealthRecord = mi.HealthRecord.Place,
                        MedicalInstructionTypeId = mi.MedicalInstructionTypeId,
                        MedicalInstructionTypeName = mi.MedicalInstructionType.Name,
                        Description = mi.Description,
                        Conclusion = mi.Conclusion,
                        DateCreate = mi.DateCreate,
                        Status = mi.Status,
                        AppointmentId = mi.AppointmentId,
                        Diseases = mi.Diseases.Select(x => x.DiseaseId + "-" + x.Name).ToList() ,
                        DateTreatement = mi.DateTreatment,
                    };
                    if(mi.MedicalInstructionImages.Any())
                    {
                        tmp.Images = mi.MedicalInstructionImages.Select(x => x.Image).ToList();
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
                        if(mi.MedicalInstructionTypeId == 8)
                        {
                            tmp.VitalSignScheduleRespone = new MedicalInstructionInformation.VitalSignSchedule()
                            {
                                TimeStared = mi.VitalSignSchedule.DateStarted,
                                TimeCanceled = mi.VitalSignSchedule.DateCanceled.GetValueOrDefault(),
                                VitalSigns = mi.VitalSignSchedule.VitalSigns
                                .Select(x => new MedicalInstructionInformation.VitalSignSchedule.VitalSign()
                                {
                                    VitalSignTypeId = x.VitalSignTypeId,
                                    VitalSignType = x.VitalSignType.VitalSignName,
                                    MinuteDangerInterval = x.MinuteDangerInterval,
                                    MinuteNormalInterval = x.MinuteNormalInterval,
                                    NumberMax = x.NumberMax,
                                    NumberMin = x.NumberMin,
                                    MinuteAgain = x.MinuteAgain,
                                    TimeStart = x.TimeStart
                                }).ToList()
                            };
                        }                        
                    }
                    return tmp;
                }
            }
            return null;
        }

        public async Task<ICollection<MedicalInstructionByDiseaseRespone>> GetMIToCreateContract(int patientId,string diseaseId,int? medicalInstructionTypeId,ICollection<int> medicalInstructionIds)
        {
            if (patientId != 0)
            {
                if (medicalInstructionIds.Any())
                {
                    medicalInstructionIds = medicalInstructionIds.Distinct().ToList();
                }
                var mis = await _repoMI.GetDbSet().Include(x => x.HealthRecord).ThenInclude(x => x.Diseases)
                    .Include(x => x.MedicalInstructionImages)
                    .Include(x => x.Diseases)
                    .Include(x => x.VitalSignSchedule)
                    .Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId 
                    && (x.Diseases.Any() && !string.IsNullOrEmpty(diseaseId) ? x.Diseases.Any(y => y.DiseaseId.Equals(diseaseId)) : true) 
                    && !x.Status.Equals("DELETE") && !x.Status.Equals("CONTRACT") && !x.Status.Equals("PENDING") && (!string.IsNullOrEmpty(diseaseId) ? x.HealthRecord.Diseases.Any(y => y.DiseaseId.Equals(diseaseId)) : true) && (medicalInstructionIds.Any() ? !medicalInstructionIds.Any(y => y == x.MedicalInstructionId): true) &&(medicalInstructionTypeId != null ? x.MedicalInstructionTypeId == medicalInstructionTypeId : true) && x.MIShareFromId == null).ToListAsync();
                // remove vital sign when that not finished
                foreach(var tmp in mis)
                {
                    if(tmp.MedicalInstructionTypeId == 8 && tmp.VitalSignScheduleId != null)
                    {
                        if (!tmp.VitalSignSchedule.Status.Equals("CANCEL"))
                        {
                            mis.Remove(tmp);
                            break;
                        }
                    }
                }
                if(mis.Any())
                {
                    var respone = new List<MedicalInstructionByDiseaseRespone>();
                    foreach(var tmp in mis.GroupBy(x => x.HealthRecord))
                    {
                        var entity = new MedicalInstructionByDiseaseRespone()
                        {
                            HealthRecordPlace = tmp.Key.Place,
                            DateCreated = tmp.Key.DateCreated.ToString("dd/MM/yyyy"),
                            Diseases = tmp.Key.Diseases.Select(x => x.DiseaseId+"-"+x.Name).ToList(),
                            MedicalInstructions = tmp.Select(x => new MedicalInstructionByDiseaseRespone.MedicalInstruction()
                            {
                                MedicalInstructionId = x.MedicalInstructionId,
                                Disease = x.Diseases.Any() ? x.Diseases.Aggregate<Disease,string>("",(str,s) => str += s.DiseaseId +"-"+ s.Name +"/"): null,
                                Images = x.MedicalInstructionImages.Select(x => x.Image).ToList(),
                                DateTreatment = x.DateTreatment != null ? x.DateTreatment.GetValueOrDefault().ToString("dd/MM/yyyy") : null,
                                Conclusion= x.Conclusion
                            }).OrderBy(x => x.DateTreatment).ToList()
                        };
                        respone.Add(entity);
                    }
                return respone;                   
                }               
            }
            return null;
        }

        public async Task<ICollection<MedicalInstructionOverviewRespone>> GetMedicalInstructionsByHRId(int healthRecordId, int? medicalInstructionTypeId)
        {
            if (healthRecordId != 0)
            {
                var hrs = await _repoMI.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && !x.Status.Equals("DELETE") && (medicalInstructionTypeId != null ? x.MedicalInstructionTypeId == medicalInstructionTypeId : true))
                    .Include(x => x.HealthRecord).Include(x => x.MedicalInstructionImages)
                    .Include(x => x.MedicalInstructionType)
                    .Include(x => x.Diseases)
                    .OrderByDescending(x => x.DateCreate).ToListAsync();
                if (hrs.Any())
                {
                    var respone = hrs.Select(x => new MedicalInstructionOverviewRespone()
                    {
                        MedicalInstructionId = x.MedicalInstructionId,
                        DateCreate = x.DateCreate,
                        DateTreatment = x.DateTreatment,
                        Description = x.Description,
                        Conclusion = x.Conclusion,
                        MedicalInstructionTypeName = x.MedicalInstructionType.Name,
                        MedicalInstructionTypeId = x.MedicalInstructionTypeId,
                        Status = x.Status,
                        Images = x.MedicalInstructionImages.Any() ? x.MedicalInstructionImages.Select(y => y.Image).ToList() : null,
                        Diseases = x.Diseases.Any() ? x.Diseases.Select(x => x.DiseaseId +"-" +x.Name).ToList() : null
                    }).ToList();
                    return respone;
                }                          
            }
            return null;
        }

        public async Task<ICollection<MedicalInstructionToShareRespone>> GetMedicalInstructionsToShare(int patientId, int healthRecordId,int? medicalInstructionTypeId)
        {
            if (patientId != 0 && healthRecordId != 0)
            {
                // get all MI of patient by patientId
                var mis = await _repoMI.GetDbSet().Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId && !x.Status.Equals("DELETE") && (medicalInstructionTypeId != null ? x.MedicalInstructionTypeId == medicalInstructionTypeId : true))
                    .Include(x => x.MedicalInstructionImages).Include(x => x.HealthRecord)
                    .Include(x => x.MedicalInstructionType).Include(x => x.VitalSignSchedule)
                    .Include(x => x.Prescription)
                    .ToListAsync();
                if (mis.Any())
                {
                    // Get vitalsignSchedule check active to remove
                    var vitalSigns = mis.Where(x => x.MedicalInstructionTypeId == 8 && x.VitalSignScheduleId != null && x.VitalSignSchedule.Status.Equals("ACTIVE")).ToList();
                    mis = mis.Except(vitalSigns).ToList();
                    /*
                    var prescription = mis.Where(x => x.MedicalInstructionTypeId == 1 && x.Prescription != null && x.Prescription.Status.Equals("ACTIVE")).ToList();
                    mis.Except(prescription);
                    
                    if (vitalSigns.Any())
                    {
                        foreach (var tmp in vitalSigns)
                        {
                            mis.Remove(tmp);
                        }
                    }    
                    */
                    // Get allMI of HRid 
                    var misHR = mis.Where(x => x.HealthRecordId == healthRecordId).ToList();
                    // Get allMi except HRid = 
                    var misNotHR = mis.Where(x => x.HealthRecordId != healthRecordId && !x.Status.Equals("PENDING") && !x.Status.Equals("CONTRACT")).ToList();
                    // Except medicalInstruc shared have MIFromId
                    misNotHR = misNotHR.Where(x => !misHR.Select(y => y.MIShareFromId).Contains(x.MedicalInstructionId)).ToList();

                    var respone = misNotHR.GroupBy(x => x.HealthRecord.Place) // Group by HealthRecordPlace
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
                               Images = m.MedicalInstructionImages.Select(x => x.Image).ToList(),
                               DateCreate = m.DateCreate.ToString("dd/MM/yyyy"),
                               Conclusion = m.Conclusion,
                               Status = m.Status,
                               MedicalInstructionTypeId = m.MedicalInstructionTypeId
                           }).ToList()
                       }).ToList()
                   }).ToList();
                    if (respone.Any())
                    {
                        return respone;
                    }
                }
            }
            return null;
        }

        // Prescription Service
        public async Task<ICollection<MedicalInstructionInformation>> GetPrescriptionByPatientId(int patientId, int? healthRecordId)
        {
            if(patientId != 0)
            {
                var tmp = await _repoMI.GetDbSet().Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId && x.MedicalInstructionTypeId == 1 && x.HealthRecord.Contract.Status.Equals("ACTIVE") && (healthRecordId != null ? x.HealthRecordId == healthRecordId : healthRecordId == null) && x.Status.Equals("DOCTOR")).OrderByDescending(x => x.DateCreate).Select(x => new MedicalInstructionInformation()
                {
                    MedicalInstructionId = x.MedicalInstructionId,
                    MedicalInstructionTypeId = x.MedicalInstructionTypeId,
                    MedicalInstructionTypeName = x.MedicalInstructionType.Name,
                    Conclusion = x.Conclusion,                    
                    Description = x.Description,
                    PlaceHealthRecord = x.HealthRecord.Place,
                    Diseases = x.Diseases.Select(y => y.DiseaseId+"-"+y.Name).ToList(),
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
                        mi.Prescription.DateCanceled = DateTime.Now;
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
                        DateStarted = request.DateStart.GetValueOrDefault(),
                        DateFinished = request.DateFinish.GetValueOrDefault(),
                        MedicationSchedules = medicationSchedules,
                        Status = "ACTIVE"
                    };
                    // mapping MI with disease
                    
                    var medicalInstruction = new MedicalInstruction()
                    {
                        HealthRecordId = request.HealthRecordId,
                        DateCreate = DateTime.Now,
                        DateTreatment = DateTime.Now,
                        MedicalInstructionTypeId = 1,
                        Description = request.Description,
                        Conclusion = request.Conclusion,
                        Prescription = prescription,
                        Status = "DOCTOR",
                        AppointmentId = request.AppointmentId
                    };
                    if (request.DiseaseIds.Any())
                    {
                        medicalInstruction.Diseases = await _repoDisease.GetDbSet().Where(x => request.DiseaseIds.Contains(x.DiseaseId)).ToListAsync();
                        // insert disease to HR if it's new
                        var healthRecord = await _repoHR.GetDbSet().Include(x => x.Diseases)
                            .Where(x => x.HealthRecordId == request.HealthRecordId).FirstOrDefaultAsync();
                        if (healthRecord != null)
                        {
                            var tmp = request.DiseaseIds.Where(x => !healthRecord.Diseases.Any(y => y.DiseaseId.Equals(x))).ToList();
                            if (tmp.Any())
                            {
                                var diseases = await _repoDisease.GetDbSet().Where(x => tmp.Any(y => y.Equals(x.DiseaseId))).ToListAsync();
                                if (diseases.Any())
                                {
                                    healthRecord.Diseases = healthRecord.Diseases.Union(diseases).ToList();
                                    await _repoHR.Update(healthRecord);
                                }
                            }
                        }
                    }
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
                // cancel all vitalSignSchedule have statc "ACTIVE"
                var tmp = await _repoMI.GetDbSet().Where(x => x.HealthRecordId == request.HealthRecordId && x.MedicalInstructionTypeId == 8).Select(x => x.VitalSignSchedule).Where(x => x.Status.Equals("ACTIVE")).ToListAsync();
                // update to CANCEL 
                var currentDate = DateTime.Now;
                if (tmp.Any())
                {
                    tmp.ForEach(x => { x.Status = "CANCEL"; x.DateCanceled = currentDate; });
                    await _repoVSSchedule.UpdateRange(tmp);
                }                              
                // Create vitalSign with Type and insert vitalSignValue
                var vitalSigns = request.VitalSigns.Select(x => new Data.Models.VitalSign()
                {
                    NumberMax = x.NumberMax,
                    NumberMin = x.NumberMin,
                    MinuteDangerInterval = x.MinuteDangerInterval,
                    MinuteNormalInterval = x.MinuteNormalInterval,
                    TimeStart = x.TimeStart,
                    MinuteAgain = x.MinuteAgain,
                    VitalSignTypeId = x.VitalSignTypeId,                              
                }).ToList();
                //Create medicalInstruction and vitalsignSchedule
                var medicalInstruction = new MedicalInstruction()
                {
                    HealthRecordId = request.HealthRecordId,
                    DateCreate = currentDate,
                    DateTreatment = currentDate,
                    MedicalInstructionTypeId = 8,
                    Description = request.Description,
                    Conclusion = request.Conclusion,                    
                    Status = "DOCTOR",
                    AppointmentId = request.AppointmentId,
                    VitalSignSchedule = new VitalSignSchedule()
                    {
                        DateStarted = currentDate,
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

        public async Task<bool> ShareMedicalInstructions(int healthRecordId, ICollection<int> medicalInstructionIds)
        {
            if (healthRecordId != 0 && medicalInstructionIds.Any())
            {
                var dateNow = DateTime.Now;

                var mis = await _repoMI.GetDbSet().Include(x => x.MedicalInstructionImages).
                    Where(x => medicalInstructionIds.Any(y => y == x.MedicalInstructionId))
                    .Select(x => new MedicalInstruction()
                    {
                        DateCreate = dateNow,
                        DateTreatment = dateNow,
                        Description = x.Description,
                        Conclusion = x.Conclusion,
                        MedicalInstructionImages = x.MedicalInstructionImages,
                        MedicalInstructionTypeId = x.MedicalInstructionTypeId,
                        VitalSignScheduleId = x.VitalSignScheduleId,
                        PrescriptionId = x.PrescriptionId,
                        Status = "PENDING",
                        HealthRecordId = healthRecordId,
                        MIShareFromId = x.MedicalInstructionId,
                        Diseases = x.Diseases,
                    }).ToListAsync();
                if (mis.Any())
                {
                    if (await _repoMI.InsertRange(mis))
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<ICollection<VitalSignScheduleRespone>> GetVitalSignScheduleByPatientId(int patientId, string? status)
        {
            if(patientId != 0)
            {
                var vitalSigns = await _repoMI.GetDbSet().Where(x => x.HealthRecord.PersonalHealthRecord.PatientId == patientId && x.MedicalInstructionTypeId == 8 && (status != null ? x.VitalSignSchedule.Status.Equals(status.ToUpper()) : true)).Include(x => x.VitalSignSchedule).ThenInclude(x => x.VitalSigns).ThenInclude(x => x.VitalSignType)
                    .Select(x => new VitalSignScheduleRespone()
                    {
                        DoctorAccountId = x.HealthRecord.Contract.Doctor.AccountId,
                        PatientAccountId = x.HealthRecord.Contract.Patient.AccountId,
                        MedicalInstructionId = x.MedicalInstructionId,
                        VitalSignScheduleId = x.VitalSignSchedule.VitalSignScheduleId,
                        DateStarted = x.VitalSignSchedule.DateStarted,
                        DateCanceled = x.VitalSignSchedule.DateCanceled,
                        Status = x.VitalSignSchedule.Status,
                        VitalSigns = x.VitalSignSchedule.VitalSigns.Any() ? x.VitalSignSchedule.VitalSigns.Select(y => new VitalSignScheduleRespone.VitalSign()
                        {
                            VitalSignType = y.VitalSignType.VitalSignName,
                            NumberMax = y.NumberMax,
                            NumberMin = y.NumberMin,
                            TimeStart = y.TimeStart,
                            MinuteAgain = y.MinuteAgain,
                            MinuteDangerInterval = y.MinuteDangerInterval,
                            MinuteNormalInterval = y.MinuteNormalInterval
                        }).ToList() : null
                    }).ToListAsync();
                if (vitalSigns.Any())
                {
                    return vitalSigns;
                }
            }
            return null;
        }

        public async Task<bool> DeleteMedicalInstruction(int medicalInstructionId)
        {
            if(medicalInstructionId != 0)
            {
                var mi = await _repoMI.GetById(medicalInstructionId);
                if(mi != null)
                {
                    if (mi.Status.Equals("PATIENT") || mi.Status.Equals("PENDING"))
                    {
                        mi.Status = "DELETE";
                        if (await _repoMI.Update(mi))
                        {
                            await _uow.CommitAsync();
                            return true;
                        }
                    }                   
                }
            }
            return false;
        }

        public async Task<bool> AddMedicalInstructionFromContract(int contractId, ICollection<int> medicalInstructionIds)
        {
            if(contractId != 0 && medicalInstructionIds.Any())
            {
                // Get midicalInstruction already exist HealthRecord
                var mis = await _repoMI.GetDbSet().Where(x => x.HealthRecord.ContractId == contractId && (x.HealthRecord.Contract.Status.Equals("APPROVED") || x.HealthRecord.Contract.Status.Equals("SIGNED"))).ToListAsync();
                // Get medicalInstruction Choose 
                var misChoose = await _repoMI.GetDbSet().Include(x => x.MedicalInstructionImages).Where(x => medicalInstructionIds.Any(y => y == x.MedicalInstructionId)).ToListAsync();

                // tra ve 1 tap khong co trong mi HR
                misChoose.Except(mis);
                var listTmp = misChoose.Where(x => !mis.Any(y => y.MIShareFromId == x.MedicalInstructionId)).ToList();
                if (listTmp.Any())
                {
                    var miInserts = listTmp.Select(x => new MedicalInstruction() {
                        Diseases = x.Diseases,
                        DateCreate = DateTime.Now,
                        DateTreatment = x.DateTreatment,
                        Description = x.Description,
                        Conclusion = x.Conclusion,
                        HealthRecordId = mis.FirstOrDefault().HealthRecordId,
                        MedicalInstructionImages = x.MedicalInstructionImages,
                        MedicalInstructionTypeId = x.MedicalInstructionTypeId,
                        MIShareFromId = x.MedicalInstructionId,
                        PrescriptionId = x.PrescriptionId,
                        VitalSignScheduleId = x.VitalSignScheduleId,
                        Status = "CONTRACT",                       
                    }).ToList();
                    await _repoMI.InsertRange(miInserts);
                }
                // Lay trung giua 2 chuoi
                //listTmp = misChoose.Where(x => mis.Any(y => x.MIShareFromId == y.MedicalInstructionId)).ToList();
                var tmp = mis.Where(x => misChoose.Any(y => y.MedicalInstructionId == x.MIShareFromId)).Select(x => {
                    if (x.Status.Equals("DELETE")) x.Status = "CONTRACT";
                    return x;
                }).ToList();
                var tmp2 = mis.Where(x => !misChoose.Any(y => y.MedicalInstructionId == x.MIShareFromId)).Select(x => {
                    if (x.Status.Equals("CONTRACT")) x.Status = "DELETE";
                    return x;
                }).ToList();

                if(await _repoMI.UpdateRange(mis))
                {
                    await _uow.CommitAsync();
                    return true;
                }                
            }
            return false;
        }
        public async Task<bool> UpdateStatusMedicalInstruction(int medicalInstructionId,string status)
        {
            if(medicalInstructionId != 0 && !string.IsNullOrEmpty(status))
            {
                var mi = await _repoMI.GetById(medicalInstructionId);
                if(mi != null)
                {
                    mi.Status = status;
                }
                if(await _repoMI.Update(mi))
                {
                    await _uow.CommitAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
