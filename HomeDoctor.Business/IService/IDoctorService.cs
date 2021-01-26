using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IDoctorService
    {
        public Task<bool> Login(string username, string password);
        public Task<DoctorInformation> GetDoctorInformation(string? username,int? doctorId);
    }
}
