using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryBase<Appointment> _repo;
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Appointment>();
            _repoMI = _uow.GetRepository<MedicalInstruction>();
        }

        public async Task<bool> CancelAppointmentByPatient(int appointmentId,string reasonCancel)
        {
            if(appointmentId != 0)
            {
                var appointment = await _repo.GetById(appointmentId);
                if(appointment != null)
                {
                    appointment.Status = "CANCEL";
                    if (!string.IsNullOrEmpty(reasonCancel))
                    {
                        appointment.ReasonCanceled = reasonCancel;
                    }
                    appointment.DateCanceled = DateTime.Now;
                    var check = await _repo.Update(appointment);
                    if (check)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }

            }
            return false;
        }

        public async Task<int> CreateAppointment(AppointmentCreate request)
        {
            if(request != null)
            {
                var currentTime = DateTime.Now;
                var appointment = new Appointment()
                {
                   DateExamination = request.DateExamination,
                   HealthRecordId = request.HealthRecordId,
                   Note = request.Note,
                   Status = "ACTIVE",
                   DateCreated = currentTime
                };
                if (await _repo.Insert(appointment))
                {
                    await _uow.CommitAsync();
                    return appointment.AppointmentId;
                }
            }
            return 0;
        }
        public async Task<ICollection<AppointmentForMonth>> GetAppointmentForMonth(int accountId,DateTime? month)
        {
            if (accountId != 0)
            {
                var appointments = await _repo.GetDbSet().Where(x => (month != null ? (x.DateExamination.Year == month.Value.Year && x.DateExamination.Month == month.Value.Month) : true) 
                && (x.HealthRecord.Contract.Patient.AccountId == accountId || x.HealthRecord.Contract.Doctor.AccountId == accountId))
                    .Include(x => x.MedicalInstructions).ThenInclude(x => x.MedicalInstructionType)
                    .Include(x => x.HealthRecord).ThenInclude(x => x.Contract).ToListAsync();
                if (appointments.Any())
                {
                    var respone = appointments.OrderBy(x => x.DateExamination)
                        .GroupBy(x => x.DateExamination.Date)
                        .Select(x => new AppointmentForMonth()
                        {
                            DateExamination = x.Key.ToString("dd/MM/yyyy"),
                            Appointments = x.Select(y => new AppointmentForMonth.Appointment() {
                                AppointmentId = y.AppointmentId,
                                MedicalInstructions = y.MedicalInstructions != null ? y.MedicalInstructions.Select(x => new AppointmentForMonth.MedicalInstruction() {
                                    MedicalInstructionId = x.MedicalInstructionId,
                                    MedicalInstructionType = x.MedicalInstructionType.Name
                                }).ToList() : null,
                                FullNameDoctor = y.HealthRecord.Contract.FullNameDoctor,
                                FullNamePatient = y.HealthRecord.Contract.FullNamePatient,
                                DateExamination = y.DateExamination,
                                Note = y.Note,
                                Status = y.Status,
                                DateCanceled = y.DateCanceled,
                                ReasonCanceled = y.ReasonCanceled
                            }).OrderBy(x => x.DateExamination).ToList()
                        }).ToList();
                    if (respone.Any())
                    {
                        return respone;
                    }
                }                
            }
            return null;
        }
        public async Task<bool> UpdateAppointment(int appointmentId, DateTime? dateExamination,string diagnose, string status)
        {
            if(appointmentId != 0)
            {
                var appointment = await _repo.GetById(appointmentId);
                if(appointment != null)
                {
                    if(dateExamination != null)
                    {
                        if (appointment.Status.Equals("ACTIVE"))
                        {
                            appointment.DateExamination = dateExamination.GetValueOrDefault();
                        }
                    }
                    if(!string.IsNullOrEmpty(status))
                    {
                        appointment.Status = status;
                    }
                    if(!string.IsNullOrEmpty(diagnose))
                    {
                        appointment.Diagnose = diagnose;
                    }
                    var check = await _repo.Update(appointment);
                    if (check)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }                             
            }
            return false;
        }
        public async Task<AppointmentDetailRespone> GetAppointmentById(int appointmentId)
        {
            if(appointmentId != 0)
            {
                var appointment = await _repo.GetDbSet().Where(x => x.AppointmentId == appointmentId).Select(x => new AppointmentDetailRespone()
                {
                    DateCanceled = x.DateCanceled,
                    DateExamination = x.DateExamination,
                    Note = x.Note,
                    PatientId = x.HealthRecord.Contract.PatientId,
                    FullNameDoctor = x.HealthRecord.Contract.FullNameDoctor,
                    FullNamePatient = x.HealthRecord.Contract.FullNamePatient,
                    ReasonCanceled = x.ReasonCanceled,
                    Status = x.Status,
                    Diagnose = x.Diagnose,
                    MedicalInstructions = x.MedicalInstructions != null ? x.MedicalInstructions.Select(y => new AppointmentDetailRespone.MedicalInstruction()
                    {
                        MedicalInstructionId = y.MedicalInstructionId,
                        MedicalInstructionType = y.MedicalInstructionType.Name,
                        DateCreated = y.DateCreate
                    }).ToList() : null
                }).FirstOrDefaultAsync();
                if(appointment != null)
                {
                    return appointment;
                }
            }
            return null;
        }

        public async Task<ICollection<AppointmentDetailRespone>> GetAppointmentsBetweenDoctorAndPatient(int healthRecordId, string status)
        {
           if(healthRecordId != 0)
            {
                var appointments = await _repo.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && (!string.IsNullOrEmpty(status) ? x.Status.Equals(status) : true)).Select(x => new AppointmentDetailRespone() {
                    DateCanceled = x.DateCanceled,
                    DateExamination = x.DateExamination,
                    Diagnose = x.Diagnose,
                    PatientId = x.HealthRecord.Contract.PatientId,
                    FullNameDoctor = x.HealthRecord.Contract.FullNameDoctor,
                    FullNamePatient = x.HealthRecord.Contract.FullNamePatient,
                    MedicalInstructions = x.MedicalInstructions != null ? x.MedicalInstructions.Select(y => new AppointmentDetailRespone.MedicalInstruction() {
                        DateCreated = y.DateCreate,
                        MedicalInstructionId = y.MedicalInstructionId,
                        MedicalInstructionType = y.MedicalInstructionType.Name
                    }).ToList() : null,
                    Note = x.Note,
                    ReasonCanceled = x.ReasonCanceled,
                    Status = x.Status
                }).ToListAsync();
                if (appointments.Any())
                {
                    return appointments;
                }
            }
            return null;
        }
        public async Task<string> CheckAppointmentToCreate(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var currentDateTime = DateTime.Now;
                var appointments = await _repo.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && x.Status.Equals("ACTIVE")).FirstOrDefaultAsync();
                if (appointments != null)
                {
                    return appointments.DateExamination.ToString("dd/MM/yyyy");
                }
            }
            return null;           
        }
    }
}
