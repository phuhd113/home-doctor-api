using HomeDoctor.Business.ViewModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IPatientService
    {
        public Task<PatientInformation> GetPatientInformation(int patientId);

        public Task<ICollection<PatientTrackingRespone>> GetPatientTrackingByDoctor(int doctorId);

        public Task<ICollection<PatientInformation>> GetAllPatient();
    }
}
