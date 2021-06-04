using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IAppointmentService
    {
        public Task<int> CreateAppointment(AppointmentCreate request);
        public Task<ICollection<AppointmentForMonth>> GetAppointmentForMonth(int accountId, DateTime? month);
        public Task<bool> UpdateAppointment(int appointmentId, DateTime? dateExamination,string diagnose, string status);
        public Task<bool> CancelAppointmentByPatient(int appointmentId,string reasonCancel);
        public Task<AppointmentDetailRespone> GetAppointmentById(int appointmentId);
        public Task<ICollection<AppointmentDetailRespone>> GetAppointmentsBetweenDoctorAndPatient(int healthRecordId, string status);
        public Task<string> CheckAppointmentToCreate(int healthRecordId);
    }
}
