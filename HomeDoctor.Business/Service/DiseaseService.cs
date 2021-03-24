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
    public class DiseaseService : IDiseaseService
    {
        private readonly IRepositoryBase<Disease> _repo;
        private readonly IUnitOfWork _uow;

        public DiseaseService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Disease>();
        }

        public async Task<DiseaseRespone> GetDiseaseById(string diseaseId)
        {
            if (!string.IsNullOrEmpty(diseaseId))
            {
                var disease = await _repo.GetDbSet().Where(x => x.DiseaseId.Equals(diseaseId)).Select(x => new DiseaseRespone()
                {
                    DiseaseId = x.DiseaseId,
                    NameDisease = x.Name
                }).FirstOrDefaultAsync();
                if (disease != null)
                {
                    return disease;
                }
            }
            return null;
        }

        public async Task<ICollection<DiseaseRespone>> GetDiseases()
        {
            var diseases = await _repo.GetDbSet().Where(x => x.Number == null).Select(x => new DiseaseRespone() { 
                DiseaseId = x.DiseaseId,
                NameDisease = x.Name
            }).ToListAsync();
            if (diseases.Count != 0)
            {
                return diseases;
            }
            return null;
        }

        public async Task<ICollection<DiseaseByPatientRespone>> GetDiseasesToCreateContract(int patientId)
        {
            if(patientId != 0)
            {
                // Get all HearDiseaseLever2 
                var heartDiseases = await _repo.GetDbSet().Where(x => x.Number == null && x.Code.Equals("I")).ToListAsync();

                // Get HeartDisease that patient has
                var diseases = await _repo.GetDbSet().Where(x => x.HealthRecords.Any(y => y.PersonalHealthRecord.PatientId == patientId  // Of patient
                && y.Diseases.Any(z => z.Code.Equals("I")))).ToListAsync(); // by hearDisease
                if(diseases.Count != 0)
                {
                    var respon = new List<DiseaseByPatientRespone>();
                    foreach (var n in heartDiseases)
                    {
                        var check = diseases.Where(x => x.Number >= n.Start && x.Number <= n.End).ToList();
                        if (check.Count != 0)
                        {
                            var disease = new DiseaseByPatientRespone()
                            {
                                DiseaseLevelTwoId = n.DiseaseId,
                                DiseaseLeverTwoName = n.Name,
                                DiseaseLeverThrees = check.Select(x => new DiseaseByPatientRespone.DiseaseLeverThree()
                                {
                                    DiseaseLevelThreeId = x.DiseaseId,
                                    DiseaseLeverThreeName = x.Name
                                }).ToList()
                            };
                            respon.Add(disease);
                        }
                    }
                    if(respon.Count != 0)
                    {
                        return respon;
                    }
                }
            }
            return null;
        }

        public async Task<ICollection<DiseaseByPatientRespone>> GetHeartDiseases()
        {
            // Get all HearDiseaseLever2 
            var heartDiseases = await _repo.GetDbSet().Where(x => x.Number == null && x.Code.Equals("I")).ToListAsync();
            // Get HeartDiseaseLever3
            var diseases = await _repo.GetDbSet().Where(x => x.Number != null && x.Code.Equals("I")).ToListAsync();
            if (diseases.Count != 0)
            {
                var respon = new List<DiseaseByPatientRespone>();
                foreach (var n in heartDiseases)
                {
                    var check = diseases.Where(x => x.Number >= n.Start && x.Number <= n.End).ToList();
                    if (check.Count != 0)
                    {
                        var disease = new DiseaseByPatientRespone()
                        {
                            DiseaseLevelTwoId = n.DiseaseId,
                            DiseaseLeverTwoName = n.Name,
                            DiseaseLeverThrees = check.Select(x => new DiseaseByPatientRespone.DiseaseLeverThree()
                            {
                                DiseaseLevelThreeId = x.DiseaseId,
                                DiseaseLeverThreeName = x.Name
                            }).ToList()
                        };
                        respon.Add(disease);
                    }
                }
                if (respon.Count != 0)
                {
                    return respon;
                }
            }
            return null;
        }

        public async Task<bool> InsertDisease(string diseaseId, string code, int? number, int? start, int? end, string name)
        {
            var disease = new Disease()
            {
                DiseaseId = diseaseId,
                Code = code,
                Number = number,
                Start = start,
                End = end,
                Name = name
            };
            var check = await _repo.Insert(disease);
            if (check)
            {
                await _uow.CommitAsync();
                return true;
            }
            return false;
        }

      
    }
}
