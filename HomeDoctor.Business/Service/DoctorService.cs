using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepositoryBase<Doctor> _repo;
        private readonly IUnitOfWork _uow;

        public DoctorService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Doctor>();
        }

        public async Task<DoctorInformation> GetDoctorInformation(string? username, int? doctorId)
        {
            Doctor tmp = null;
            if (username != null)
            {
                tmp = _repo.GetDbSet().Include(x => x.Account).FirstOrDefaultAsync(x => x.Username.Equals(username)).Result;
            }
            if(doctorId != 0)
            {
                tmp = _repo.GetDbSet().Include(x => x.Account).FirstOrDefaultAsync(x => x.DoctorId == doctorId).Result;
            }           
            if (tmp != null)
            {
                var doctorInfor = new DoctorInformation()
                {
                    DateOfBirth = tmp.Account.DateOfBirth,
                    Email = tmp.Account.Email,
                    FullName = tmp.Account.FullName,
                    Phone = tmp.Account.PhoneNumber,
                    Username = tmp.Username,
                    WorkLocation = tmp.WorkLocation
                };
                return doctorInfor;
            } 
            return null;
        }

        public async Task<bool> Login(string username, string password)
        {
            var check = _repo.GetDbSet().FirstOrDefaultAsync(x => x.Username.Equals(username) && x.Password.Equals(password)).Result;        
            if(check != null)
            {
                return true;
            }
            return false;
        }
    }
}
