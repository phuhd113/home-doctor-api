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
    public class DoctorService : IDoctorService
    {
        private readonly IRepositoryBase<Doctor> _repo;
        private readonly IRepositoryBase<Contract> _repoContract;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _uow;

        public DoctorService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Doctor>();
            _repoContract = _uow.GetRepository<Contract>();
            _repoAccount = _uow.GetRepository<Account>();
        }

        public async Task<DoctorInformation> GetDoctorInformation(int doctorId)
        {    
            if(doctorId != 0)
            {
                var doctor = await _repo.GetDbSet().Include(x => x.Account).FirstOrDefaultAsync(x => x.DoctorId == doctorId);
                if (doctor != null)
                {
                    var doctorInfor = new DoctorInformation()
                    {
                        DoctorId = doctor.DoctorId,
                        AccountId = doctor.AccountId,
                        DateOfBirth = doctor.Account.DateOfBirth,
                        Email = doctor.Account.Email,
                        FullName = doctor.Account.FullName,
                        Phone = doctor.Account.PhoneNumber,
                        Username = doctor.Account.Username,
                        WorkLocation = doctor.WorkLocation,
                        Experience = doctor.Experience,
                        Details = doctor.Details,
                        Specialization = doctor.Specialization,
                        Address = doctor.Account.Address                      
                    };
                    return doctorInfor;
                }
            }               
            return null;
        }

        public async Task<ICollection<DoctorTrackingRespone>> GetDoctorTrackingByPatientId(int patientId)
        {
            if (patientId != 0)
            {
                var respone = await _repoContract.GetDbSet().Where(x => x.PatientId == patientId && x.Status.Equals("ACTIVE")).Include(x => x.Doctor).Include(x => x.HealthRecord)
                    .Select(x => new DoctorTrackingRespone()
                {
                    AccountDoctorId = x.Doctor.AccountId,
                    DoctorId = x.DoctorId,
                    ContractId = x.ContractId,
                    DoctorName = x.FullNameDoctor,
                    HealthRecordId = x.HealthRecord.HealthRecordId,
                    DateContractStarted = x.DateStarted.ToString("dd/MM/yyyy")
                }).ToListAsync();
                if (respone.Any())
                {
                    return respone;
                }
            }
            return null;
        }

        public async Task<ICollection<DoctorInformation>> GetAllDoctor()
        {

            var doctors = await _repo.GetDbSet().Include(x => x.Account)
                .Select(doctor => new DoctorInformation() {
                    DoctorId = doctor.DoctorId,
                    AccountId = doctor.AccountId,
                    DateOfBirth = doctor.Account.DateOfBirth,
                    Email = doctor.Account.Email,
                    FullName = doctor.Account.FullName,
                    Phone = doctor.Account.PhoneNumber,
                    Username = doctor.Account.Username,
                    WorkLocation = doctor.WorkLocation,
                    Experience = doctor.Experience,
                    Details = doctor.Details,
                    Specialization = doctor.Specialization,
                    Address = doctor.Account.Address
                }).ToListAsync();
            if (doctors.Any())
            {
                return doctors;
            }
            return null;
        }

        public async Task<int> CreateDoctor(DoctorCreate doctor)
        {
            if(doctor != null)
            {
                var account = new Account()
                {
                    Address = doctor.Address,
                    DateCreated = DateTime.Now,
                    DateOfBirth = doctor.DateOfBirth,
                    Email = doctor.Email,
                    FullName = doctor.FullName,
                    Gender = doctor.Gender,
                    PhoneNumber = doctor.PhoneNumber,
                    RoleId = 1,
                    Username = doctor.Username,
                    Password = doctor.Password,
                    Doctor = new Doctor()
                    {
                        Experience = doctor.Experience,
                        Specialization = doctor.Specialization,
                        WorkLocation = doctor.WorkLocation,
                        Details = doctor.Details
                    }                   
                };
                if(account != null)
                {
                    if(await _repoAccount.Insert(account))
                    {
                        await _uow.CommitAsync();
                        return account.AccountId;
                    }
                }
            }
            return 0;
        }
    }
}
