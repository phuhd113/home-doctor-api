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
        private readonly IRepositoryBase<Patient> _repo;
        private readonly IRepositoryBase<Contract> _repoContract;
        private readonly IUnitOfWork _uow;
        public PatientService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Patient>();
            _repoContract = _uow.GetRepository<Contract>();
        }

        public async Task<PatientInformation> GetPatientInformation(int patientId)
        {

            if (patientId != 0)
            {
                var patient = await _repo.GetDbSet().Include(x => x.Account).Include(x => x.Relatives).FirstOrDefaultAsync(x => x.PatientId == patientId);
                if (patient != null)
                {
                    var patientInfor = new PatientInformation()
                    {
                        PatientId = patientId,
                        AccountId = patient.AccountId,
                        Address = patient.Account.Address,
                        DateOfBirth = patient.Account.DateOfBirth,
                        Email = patient.Account.Email,
                        FullName = patient.Account.FullName,
                        PhoneNumber = patient.Account.PhoneNumber,
                        Career = patient.Career,
                        Gender = patient.Account.Gender,
                        Height = patient.Height,
                        Weight = patient.Weight,
                        Relatives = patient.Relatives.Count != 0 ? patient.Relatives.Select(x => new RelativeInformation()
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

        public async Task<ICollection<PatientTrackingRespone>> GetPatientTrackingByDoctor(int doctorId)
        {
            if (doctorId != 0)
            {
                var patients = await _repoContract.GetDbSet()//.Include(x => x.Doctor).Include(x => x.Patient)
                    .Where(x => x.DoctorId == doctorId && (x.Status.Equals("ACTIVE") || x.Status.Equals("LOCKED")))/*.Include(x => x.HealthRecord).ThenInclude(x => x.MedicalInstructions)
                    .Include(x => x.HealthRecord).ThenInclude(x => x.Appointments)*/
                    .Select(x => new PatientTrackingRespone()
                    {
                        PatientId = x.PatientId,
                        PatientName = x.Patient.Account.FullName,
                        ContractStatus = x.Status,
                        PersonalStatus = x.Patient.PersonalHealthRecord.PersonalStatus,
                        DateUpdateStatus = x.Patient.PersonalHealthRecord.DateUpdateStatus,
                        DiseaseContract = x.HealthRecord.Diseases.Select(x => new PatientTrackingRespone.Disease()
                        {
                            DiseaseId = x.DiseaseId,
                            DiseaseName = x.Name
                        }).ToList(),
                        ContractId = x.ContractId,
                        HealthRecordId = x.HealthRecord.HealthRecordId,
                        AccountPatientId = x.Patient.AccountId,
                        AppointmentLast = x.HealthRecord.Appointments != null ? x.HealthRecord.Appointments.OrderByDescending(x => x.DateExamination).FirstOrDefault().DateExamination : (DateTime?)null,
                        AppointmentFirst = x.HealthRecord.AppointmentFirst,
                        VitalSignScheduleFirst = x.HealthRecord.VitalSignScheduleFirst,
                        SmartWatchConnected = x.HealthRecord.PersonalHealthRecord.SmartWatchConnected
                    }).ToListAsync();
                if (patients.Any())
                {
                    return patients;
                }
            }
            return null;
        }

        public async Task<ICollection<PatientInformation>> GetAllPatient()
        {
            var patients = await _repo.GetDbSet().Include(x => x.Account).Include(x => x.Relatives).Select(patient => new PatientInformation()
            {
                PatientId = patient.PatientId,
                AccountId = patient.AccountId,
                Address = patient.Account.Address,
                DateOfBirth = patient.Account.DateOfBirth,
                Email = patient.Account.Email,
                FullName = patient.Account.FullName,
                PhoneNumber = patient.Account.PhoneNumber,
                Career = patient.Career,
                Gender = patient.Account.Gender,
                Height = patient.Height,
                Weight = patient.Weight,
                Relatives = patient.Relatives.Count != 0 ? patient.Relatives.Select(x => new RelativeInformation()
                {
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber
                }).ToList() :
                    null
            }).ToListAsync();
            if (patients.Any())
            {
                return patients;
            }
            return null;
        }
    }
}
