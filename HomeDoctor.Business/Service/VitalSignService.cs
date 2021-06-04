using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
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
    public class VitalSignService : IVitalSignService
    {
        private readonly IRepositoryBase<VitalSignValue> _repoVSValue;
        private readonly IRepositoryBase<VitalSignSchedule> _repoVSSchedule;
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IRepositoryBase<PersonalHealthRecord> _repoPersonalHR;
        private readonly IRepositoryBase<VitalSignValueShare> _repoVSShare;
        private readonly IRepositoryBase<HealthRecord> _repoHR;
        private readonly IRepositoryBase<VitalSignType> _repoVSType;
        private readonly IUnitOfWork _uow;

        public VitalSignService(IUnitOfWork uow)
        {
            _uow = uow;
            _repoVSValue = _uow.GetRepository<VitalSignValue>();
            _repoVSSchedule = _uow.GetRepository<VitalSignSchedule>();
            _repoMI = _uow.GetRepository<MedicalInstruction>();
            _repoPersonalHR = _uow.GetRepository<PersonalHealthRecord>();
            _repoVSShare = _uow.GetRepository<VitalSignValueShare>();
            _repoHR = _uow.GetRepository<HealthRecord>();
            _repoVSType = _uow.GetRepository<VitalSignType>();
        }

        public async Task<ICollection<string>> GetDateTimeHaveVitalSignValue(int patientId, int? healthRecordId)
        {
            if(patientId != 0)
            {
                var dates = await _repoVSValue.GetDbSet().Where(x => x.PersonalHealthRecord.PatientId == patientId && (healthRecordId != null ? x.PersonalHealthRecord.HealthRecords.FirstOrDefault(x => x.HealthRecordId == healthRecordId).DateCreated.Date <= x.DateCreated.Date : true)).OrderBy(x => x.DateCreated).Select(x => x.DateCreated.ToString("dd/MM/yyyy")).ToListAsync();
                dates = dates.Distinct().ToList();
                if (dates.Any())
                {
                    return dates;
                }
            }
            return null;
        }

        public async Task<ICollection<VitalSignScheduleRespone>> GetVitalSignScheduleByHRId(int healthRecordId)
        {
           if(healthRecordId != 0)
            {
                var respone = await _repoMI.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && x.MedicalInstructionTypeId == 8 && !x.Status.Equals("CONTRACT"))
                    .Select(x => new VitalSignScheduleRespone()
                    {
                        DateStarted = x.VitalSignSchedule.DateStarted,
                        DateCanceled = x.VitalSignSchedule.DateStarted,
                        DoctorAccountId = x.HealthRecord.Contract.DoctorId,
                        PatientAccountId = x.HealthRecord.Contract.PatientId,
                        MedicalInstructionId = x.MedicalInstructionId,
                        Status = x.VitalSignSchedule.Status,
                        VitalSignScheduleId = x.VitalSignScheduleId.GetValueOrDefault(),
                        VitalSigns = x.VitalSignSchedule.VitalSigns.Select(y => new VitalSignScheduleRespone.VitalSign()
                        {
                            MinuteAgain = y.MinuteAgain,
                            MinuteDangerInterval = y.MinuteDangerInterval,
                            MinuteNormalInterval = y.MinuteNormalInterval,
                            NumberMax = y.NumberMax,
                            NumberMin = y.NumberMin,
                            TimeStart = y.TimeStart,
                            VitalSignType = y.VitalSignType.VitalSignName
                        }).ToList()
                    }
                ).ToListAsync();
                if (respone.Any())
                {
                    return respone;
                }
            }
            return null;
        }

        public async Task<VitalSignValueByMIIdRespone> GetVitalSignValueByMIId(int medicalInstructionId,int patientId)
        {
            if(medicalInstructionId != 0 && patientId != 0)
            {
                var respone = await _repoVSSchedule.GetDbSet()
                    .Where(x => x.MedicalInstructions.Any(y => y.MedicalInstructionId == medicalInstructionId))
                    .Select(x => new VitalSignValueByMIIdRespone()
                    {
                        TimeStarted = x.DateStarted,
                        TimeCanceled = x.DateCanceled,
                        Status = x.Status,
                        VitalSigns = x.VitalSigns.Select(y => new VitalSignValueByMIIdRespone.VitalSign()
                        {
                            VitalSignTypeId = y.VitalSignTypeId,
                            VitalSignType = y.VitalSignType.VitalSignName,
                            NumberMax = y.NumberMax,
                            NumberMin = y.NumberMin,
                            MinuteDangerInterval = y.MinuteDangerInterval,
                            MinuteNormalInterval = y.MinuteNormalInterval,
                            MinuteAgain = y.MinuteAgain,
                            TimeStart = y.TimeStart
                        }).ToList()
                    }).FirstOrDefaultAsync();
                if(respone != null)
                {
                    var vitalSignValues = await _repoVSValue.GetDbSet()
                        .Where(x => x.PersonalHealthRecord.PatientId == patientId && x.DateCreated.Date >= respone.TimeStarted.Date && (respone.TimeCanceled != null ? x.DateCreated.Date <= respone.TimeCanceled.GetValueOrDefault().Date : true))
                        .Select(x => new VitalSignValueByMIIdRespone.VitalSignValue()
                        {
                            DateCreated = x.DateCreated,
                            VitalSignTypeId = x.VitalSignTypeId,
                            NumberValue = x.NumberValue,
                            TimeValue = x.TimeValue
                        }).ToListAsync();
                    if (vitalSignValues.Any())
                    {
                        respone.VitalSignValues = vitalSignValues;
                    }
                    return respone;
                }
            }
            return null;
        }

        public async Task<VitalSignValuePatientIdRespone> GetVitalSignValueByPatientId(int patientId, int? healthRecordId, DateTime dateTime)
        {
            if (patientId != 0)
            {
                var vsValue = await _repoVSValue.GetDbSet().Where(x => x.PersonalHealthRecord.PatientId == patientId && x.DateCreated.Date.Equals(dateTime.Date)).ToListAsync();
                if (vsValue != null)
                {
                    var respone = new VitalSignValuePatientIdRespone()
                    {
                        VitalSignValueDateCreated = dateTime.Date,
                        VitalSignValues = vsValue.Select(x => new VitalSignValuePatientIdRespone.VitalSignValue() {
                            VitalSignTypeId = x.VitalSignTypeId,
                            DateCreated = x.DateCreated,
                            NumberValue = x.NumberValue,
                            TimeValue = x.TimeValue           
                        }).ToList()                        
                    };
                    // Get vitalSignSchedule active 
                    if (healthRecordId != null)
                    {
                        var mi = await _repoMI.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && x.MedicalInstructionTypeId == 8 && x.MIShareFromId == null && x.HealthRecord.PersonalHealthRecord.PatientId == patientId).OrderByDescending(x => x.DateCreate)
                    .Include(x => x.VitalSignSchedule.VitalSigns).ThenInclude(x => x.VitalSignType)
                    .Select(x => x.VitalSignSchedule)
                    .Where(x => x.DateStarted <= dateTime && (x.DateCanceled != null ? x.DateCanceled > dateTime : true))
                    .FirstOrDefaultAsync();
                        if (mi != null)
                        {
                            respone.VitalSigns = mi.VitalSigns
                            .Select(x => new VitalSignValuePatientIdRespone.VitalSign()
                            {
                                VitalSignTypeId = x.VitalSignTypeId,
                                VitalSignType = x.VitalSignType.VitalSignName,
                                NumberMax = x.NumberMax,
                                NumberMin = x.NumberMin,
                                MinuteDangerInterval = x.MinuteDangerInterval,
                                MinuteNormalInterval = x.MinuteNormalInterval,
                                MinuteAgain = x.MinuteAgain,
                                TimeStart = x.TimeStart
                            }).ToList();
                        }
                    }
                    return respone;
                }
            }
            return null;
        }
        /*
        public async Task<bool> InsertVitalSignEveryDay(int vitalSignScheduleId)
        {
            if(vitalSignScheduleId != 0)
            {
                var vitalSigns = await _repoVSSchedule.GetDbSet().Where(x => x.VitalSignScheduleId == vitalSignScheduleId).Select(x => x.VitalSigns).FirstOrDefaultAsync();
                if (vitalSigns != null)
                {
                    var vitalSignValues = vitalSigns.Select(x => new VitalSignValue()
                    {
                        DateCreated = DateTime.Now;,
                        VitalSignId = x.VitalSignId
                    }).ToList();
                    if (vitalSignValues.Any())
                    {
                        if(await _repoVSValue.InsertRange(vitalSignValues))
                        {
                            await _uow.CommitAsync();
                            return true;
                        }
                    }
                }              
            }
            return false;
        }
        */

        public async Task<bool> UpdateVitalSignValue(int patientId, int vitalSignTypeId,string timeValue, string numberValue)
        {
           if(patientId != 0 && vitalSignTypeId != 0)
           {
                var currentDate = DateTime.Now;
                // GetVitalSignValue today, if nothing to create,if exits to update
                var vitalSignValue = await _repoVSValue.GetDbSet().Where(x => x.PersonalHealthRecord.PatientId == patientId && x.DateCreated.Date.Equals(currentDate.Date) && x.VitalSignTypeId == vitalSignTypeId).FirstOrDefaultAsync();
                if (vitalSignValue != null)
                {
                    if(!string.IsNullOrEmpty(timeValue) && !string.IsNullOrEmpty(numberValue))
                    {
                        if(vitalSignTypeId == 1)
                        {
                            vitalSignValue.NumberValue = numberValue;
                            vitalSignValue.TimeValue = timeValue;
                        }
                        else
                        {
                            vitalSignValue.NumberValue += numberValue +",";
                            vitalSignValue.TimeValue += timeValue +",";
                        }
                    }                                  
                    if (await _repoVSValue.Update(vitalSignValue))
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
                else
                {
                    var personalHR = await _repoPersonalHR.GetDbSet().Where(x => x.PatientId == patientId).FirstOrDefaultAsync();
                    if(personalHR != null)
                    {
                        vitalSignValue = new VitalSignValue()
                        {
                            DateCreated = currentDate,
                            PersonalHealthRecordId = personalHR.PersonalHealthRecordId,
                            VitalSignTypeId = vitalSignTypeId
                        };
                        if (!string.IsNullOrEmpty(timeValue) && !string.IsNullOrEmpty(numberValue))
                        {
                            if (vitalSignTypeId == 1)
                            {
                                vitalSignValue.NumberValue = numberValue;
                                vitalSignValue.TimeValue = timeValue;
                            }
                            else
                            {
                                vitalSignValue.NumberValue = numberValue + ",";
                                vitalSignValue.TimeValue = timeValue + ",";
                            }
                        }                     
                    }                   
                    if(await _repoVSValue.Insert(vitalSignValue))
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }                              
           }
            return false;
        }

        public async Task<int> ShareVitalSignValue(int healthRecordId,DateTime timeShare,int minuteShare)
        {
            if(healthRecordId != 0 && minuteShare != 0 && timeShare != null)
            {
                if(_repoHR.GetDbSet().Any(x => x.HealthRecordId == healthRecordId))
                {
                    var vsShare = new VitalSignValueShare()
                    {
                        HealthRecordId = healthRecordId,
                        MinuteShare = minuteShare,
                        TimeShare = timeShare
                        
                    };
                    if (await _repoVSShare.Insert(vsShare))
                    {
                        await _uow.CommitAsync();
                        return vsShare.VitalSignValueShareId;
                    }
                }                
            }
            return 0;
        }

        public async Task<ICollection<VitalSignType>> GetVitalSignsType()
        {
            var respone = await _repoVSType.GetAll();
            if (respone.Any())
            {
                return respone;
            }
            return null;
        }

        public async Task<VitalSignValueShareRespone> GetVitalSignValueShareById(int vitalSignValueShareId)
        {
            if(vitalSignValueShareId != 0)
            {
                var respone = await _repoVSShare.GetDbSet().Where(x => x.VitalSignValueShareId == vitalSignValueShareId)
                    .Select(x => new VitalSignValueShareRespone()
                    {
                        VitalSignValueShareId = vitalSignValueShareId,
                        HealthRecordId = x.HealthRecordId,
                        MinuteShare = x.MinuteShare,
                        TimeShare = x.TimeShare.ToString("HH:mm dd/MM/yyyy")
                    }).FirstOrDefaultAsync();
                if(respone != null)
                {
                    return respone;
                }
            }
            return null;
        }

        public async Task<ICollection<VitalSignValueShareRespone>> GetVitalSignValueShareByDate(int healthRecordId,DateTime? dateTime)
        {
            if(healthRecordId != 0)
            {
                var respone = await _repoVSShare.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && (dateTime != null ? x.TimeShare.Date.Equals(dateTime.GetValueOrDefault().Date) : true)).OrderByDescending(x => x.TimeShare).Select(x => new VitalSignValueShareRespone()
                    {
                        VitalSignValueShareId = x.VitalSignValueShareId,
                        HealthRecordId = x.HealthRecordId,
                        MinuteShare = x.MinuteShare,
                        TimeShare = x.TimeShare.ToString("HH:mm dd/MM/yyyy")
                    }).ToListAsync();
                if (respone.Any())
                {
                    return respone;
                }
            }
            return null;
        }
    }
}
