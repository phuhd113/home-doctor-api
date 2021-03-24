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
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryBase<Appointment> _repo;
        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Appointment>();
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
                    appointment.DateCanceled = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7);
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
                var appointment = new Appointment()
                {
                    ContractId = request.ContractId,
                    DateCreated = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7),
                    DateExamination = request.DateExamination,
                    Note = request.Note,
                    Status = "PENDING"
                };
                var check = await _repo.Insert(appointment);
                if (check)
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
                && (x.Contract.Doctor.AccountId == accountId || x.Contract.Patient.AccountId == accountId)).Include(x => x.Contract).ToListAsync();
                if (appointments.Any())
                {
                    var respone = appointments.GroupBy(x => new { x.DateExamination, x.ContractId })
                        .Select(x => new AppointmentForMonth()
                        {
                            DateExamination = x.Key.DateExamination.ToString("dd/MM/yyyy"),
                            ContractId = x.Key.ContractId,
                            Appointments = x.Select(y => new AppointmentForMonth.Appointment() {
                                AppointmentId = y.AppointmentId,
                                FullNameDoctor = y.Contract.FullNameDoctor,
                                FullNamePatient = y.Contract.FullNamePatient,
                                DateExamination = y.DateExamination,
                                Note = y.Note,
                                Status = y.Status,
                                DateCanceled = y.DateCanceled,
                                ReasonCanceled = y.ReasonCanceled
                            }).OrderBy(x => x.DateExamination).ToList()
                        }).OrderBy(x => x.DateExamination).ToList();
                    if (respone.Any())
                    {
                        return respone;
                    }
                }                
            }
            return null;
        }
        public async Task<bool> UpdateAppointment(int appointmentId, DateTime? dateExamination, string status)
        {
            if(appointmentId != 0)
            {
                var appointment = await _repo.GetById(appointmentId);
                if(appointment != null)
                {
                    if(dateExamination != null)
                    {
                        if (appointment.Status.Equals("PENDING"))
                        {
                            appointment.DateExamination = dateExamination.GetValueOrDefault();
                        }
                    }
                    if(!string.IsNullOrEmpty(status))
                    {
                        appointment.Status = status;
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
    }
}
