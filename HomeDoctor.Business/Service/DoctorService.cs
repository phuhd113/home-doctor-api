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
    }
}
