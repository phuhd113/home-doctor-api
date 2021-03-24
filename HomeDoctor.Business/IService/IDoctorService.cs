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
    }
}
