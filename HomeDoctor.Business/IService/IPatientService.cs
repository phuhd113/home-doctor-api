using HomeDoctor.Business.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IPatientService
    {
        public Task<PatientInformation> GetPatientInformation(int patientId);
    }
}
