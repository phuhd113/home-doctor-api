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
    public class AccountService : IAccountService
    {
        private readonly IRepositoryBase<Account> _repo;
        private readonly IUnitOfWork _uow;

        public AccountService(IUnitOfWork uow)
        {           
            _uow = uow;
            _repo = _uow.GetRepository<Account>();
        }

        public async Task<int> GetPatientAccountIdByHRId(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var accountId = await _repo.GetDbSet().Where(x => x.Patient.PersonalHealthRecord.HealthRecords.Any(y => y.HealthRecordId == healthRecordId)).Select(x => x.AccountId).FirstOrDefaultAsync();
                if(accountId != 0)
                {
                    return accountId;
                }
            }
            return 0;
        }

        public async Task<LoginRespone> Login(string username, string password)
        {

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                var tmp = await _repo.GetDbSet().Include(x => x.Doctor).Include(x => x.Patient).Where(x => x.Username.Equals(username) && x.Password.Equals(password)).FirstOrDefaultAsync();
                if(tmp != null){
                    if (tmp.RoleId == 1)
                    {
                        return new LoginRespone
                        {
                            AccountId = tmp.AccountId,
                            Id = tmp.Doctor.DoctorId,
                            RoleId = tmp.RoleId
                        };
                    }
                    if (tmp.RoleId == 2)
                    {
                        return new LoginRespone
                        {
                            AccountId = tmp.AccountId,
                            Id = tmp.Patient.PatientId,
                            RoleId = tmp.RoleId
                        };
                    }
                }
                
            }
            return null;
        }
    }
}
