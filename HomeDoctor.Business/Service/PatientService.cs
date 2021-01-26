using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel;
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
    public class PatientService : IPatientService
    {
        private IRepositoryBase<Patient> _repo;
        private readonly IUnitOfWork _uow;       
        public PatientService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Patient>();
        }

        public async Task<PatientInformation> GetPatientInformation(int patientId)
        {

            if (patientId != 0)
            {
                var patient = _repo.GetDbSet().Include(x => x.Account).Include(x => x.Relatives).
                                FirstOrDefault(x => x.PatientId == patientId);
                if(patient != null)
                {
                    var patientInfor = new PatientInformation()
                    {
                        Address = patient.Account.Address,
                        DateOfBirth = patient.Account.DateOfBirth,
                        Email = patient.Account.Email,
                        FullName = patient.Account.FullName,
                        PhoneNumber = patient.Account.PhoneNumber,
                        Career = patient.Career,
                        DateFinished = patient.DateFinished,
                        DateStarted = patient.DateStarted,
                        Gender = patient.Account.Gender,
                        Height = patient.Height,
                        Weight = patient.Weight,
                        Relatives = patient.Relatives != null ? patient.Relatives.Select(x => new RelativeInformation()
                        {
                            FullName = x.FullName,
                            PhoneNumber = x.PhoneNumber
                        }).ToList() :
                        null
                    };                    
                    return patientInfor;
                }                            
            }
            return null;
        }
    }
}
