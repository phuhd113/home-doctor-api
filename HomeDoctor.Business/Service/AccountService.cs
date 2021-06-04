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
    public class AccountService : IAccountService
    {
        private readonly IRepositoryBase<Account> _repo;
        private readonly IRepositoryBase<HealthRecord> _repoHR;
        private readonly IUnitOfWork _uow;

        public AccountService(IUnitOfWork uow)
        {           
            _uow = uow;
            _repo = _uow.GetRepository<Account>();
            _repoHR = _uow.GetRepository<HealthRecord>();
        }

        public async Task<Dictionary<int,int>> GetAccountIdsByHRId(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var accountIds = await _repoHR.GetDbSet().Where(x => x.HealthRecordId == healthRecordId).Select(x => new
                {
                    accPatient = x.Contract.Patient.AccountId,
                    accDoctor = x.Contract.Doctor.AccountId
                }).FirstOrDefaultAsync();
                if(accountIds != null)
                {
                    var respone = new Dictionary<int, int>();
                    respone.Add(accountIds.accPatient, accountIds.accPatient);                  
                    return respone;
                }
            }
            return null;
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
                    if (tmp.RoleId == 3)
                    {
                        return new LoginRespone
                        {
                            AccountId = tmp.AccountId,
                            RoleId = tmp.RoleId
                        };
                    }
                }
                
            }
            return null;
        }

        public async Task<bool> RegisterPatient(PatientRegisterRequest request)
        {
            if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
            {
                // Create List relative
                var relatives = request.PatientInformation.PatientHealthRecord.Relatives
                    .Select(x => new Relative()
                    {
                        FullName = x.FullNameRelative,
                        PhoneNumber = x.PhoneNumber
                    }).ToList();
                // Create PersonalHealthRecord 
                var personalHR = new PersonalHealthRecord()
                {
                    FamilyMedicalHistory = request.PatientInformation.PatientHealthRecord.FamilyMedicalHistory,
                    PersonalMedicalHistory = request.PatientInformation.PatientHealthRecord.PersonalMedicalHistory,
                    SmartWatchConnected = false,
                };
                // Create Patient 
                var patient = new Patient()
                {
                    Career = request.PatientInformation.Career,
                    Height = request.PatientInformation.PatientHealthRecord.Height,
                    Weight = request.PatientInformation.PatientHealthRecord.Weight,
                    PersonalHealthRecord = personalHR,
                    Relatives = relatives,                    
                };
                // create account 
                var account = new Account()
                {
                    Username = request.Username,
                    Password = request.Password,
                    Address = request.PatientInformation.Address,
                    DateCreated = DateTime.Now,
                    DateOfBirth = request.PatientInformation.DateOfBirth,
                    Email = request.PatientInformation.Email,
                    FullName = request.PatientInformation.FullName,
                    Gender = request.PatientInformation.Gender,
                    PhoneNumber = request.PatientInformation.PhoneNumber,
                    Patient = patient,
                    RoleId = 2,
                    
                };
                
                if(await _repo.Insert(account))
                {
                    await _uow.CommitAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
