using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IAccountService
    {
        public Task<LoginRespone> Login(string username, string password);

        public Task<Dictionary<int, int>> GetAccountIdsByHRId(int healthRecordId);
        public Task<bool> RegisterPatient(PatientRegisterRequest request);
    }
}
