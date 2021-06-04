using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IDoctorService
    {
        public Task<DoctorInformation> GetDoctorInformation(int doctorId);

        public Task<ICollection<DoctorTrackingRespone>> GetDoctorTrackingByPatientId(int patientId);

        public Task<ICollection<DoctorInformation>> GetAllDoctor();
        public Task<int> CreateDoctor(DoctorCreate doctor);
    }
}
