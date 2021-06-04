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
                    DiseaseName = x.Name
                }).FirstOrDefaultAsync();
                if (disease != null)
                {
                    return disease;
                }
            }
            return null;
        }

        public async Task<ICollection<DiseasesRespone>> GetDiseases()
        {
            var diseaseTwos = await _repo.GetDbSet().Where(x => x.Number == null).ToListAsync();
            var diseaseThree = await _repo.GetDbSet().Where(x => x.Number != null).ToListAsync();
            if (diseaseTwos.Any() && diseaseThree.Any())
            {
                var respone = new List<DiseasesRespone>();
                foreach(var two in diseaseTwos)
                {

                    var tmp = new DiseasesRespone()
                    {
                        DiseaseLevelTwoId = two.DiseaseId,
                        DiseaseLevelTwoName = two.Name,
                        DiseaseLevelThrees = diseaseThree.Where(x => x.Number >= two.Start && x.Number <= two.End && x.Code.Equals(two.Code)).Select(x => new DiseasesRespone.DiseaseLevelThree()
                        {
                            DiseaseLevelThreeId = x.DiseaseId,
                            DiseaseLevelThreeName = x.Name
                        }).ToList()
                    };
                    respone.Add(tmp);                                   
                }
                if (respone.Any())
                {
                    return respone;
                }
            }          
            return null;
        }

        public async Task<ICollection<DiseasesRespone>> GetDiseasesToCreateContract(int patientId)
        {
            if(patientId != 0)
            {
                // Get all HearDiseaseLever2 
                var heartDiseases = await _repo.GetDbSet().Where(x => x.Number == null && x.Code.Equals("I")).ToListAsync();

                // Get HeartDisease that patient has
                var diseases = await _repo.GetDbSet().Where(x => x.HealthRecords.Any(y => y.PersonalHealthRecord.PatientId == patientId && !y.Status.Equals("DELETE") // Of patient
                && y.Diseases.Any(z => z.Code.Equals("I"))) && x.HealthRecords.Where(y => y.MedicalInstructions.Any() && y.MedicalInstructions.Any(z => !z.Status.Equals("DELETE") && !z.Status.Equals("PENDING") && !z.Status.Equals("CONTRACT") && !z.Status.Equals("SHARE"))).Any()).ToListAsync(); // by hearDisease
                if(diseases.Count != 0)
                {
                    var respon = new List<DiseasesRespone>();
                    foreach (var n in heartDiseases)
                    {
                        var check = diseases.Where(x => x.Number >= n.Start && x.Number <= n.End && x.Code.Equals("I")).ToList();
                        if (check.Count != 0)
                        {
                            var disease = new DiseasesRespone()
                            {
                                DiseaseLevelTwoId = n.DiseaseId,
                                DiseaseLevelTwoName = n.Name,
                                DiseaseLevelThrees = check.Select(x => new DiseasesRespone.DiseaseLevelThree()
                                {
                                    DiseaseLevelThreeId = x.DiseaseId,
                                    DiseaseLevelThreeName = x.Name
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

        public async Task<ICollection<DiseasesRespone>> GetHeartDiseases()
        {
            // Get all HearDiseaseLever2 
            var heartDiseases = await _repo.GetDbSet().Where(x => x.Number == null && x.Code.Equals("I")).ToListAsync();
            // Get HeartDiseaseLever3
            var diseases = await _repo.GetDbSet().Where(x => x.Number != null && x.Code.Equals("I")).ToListAsync();
            if (diseases.Count != 0)
            {
                var respon = new List<DiseasesRespone>();
                foreach (var n in heartDiseases)
                {
                    var check = diseases.Where(x => x.Number >= n.Start && x.Number <= n.End).ToList();
                    if (check.Count != 0)
                    {
                        var disease = new DiseasesRespone()
                        {
                            DiseaseLevelTwoId = n.DiseaseId,
                            DiseaseLevelTwoName = n.Name,
                            DiseaseLevelThrees = check.Select(x => new DiseasesRespone.DiseaseLevelThree()
                            {
                                DiseaseLevelThreeId = x.DiseaseId,
                                DiseaseLevelThreeName = x.Name
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

        public async Task<bool> InsertDiseases(ICollection<DiseaseCreate> diseases)
        {
            if (diseases.Any())
            {
                var tmp = diseases.Select(x => new Disease()
                {
                    Code = x.Code,
                    DiseaseId = x.DiseaseId,
                    End = x.End,
                    Name = x.Name,
                    Number = x.Number,
                    Start = x.Start,
                }).ToList();
                if (tmp.Any())
                {
                    var respone = await _repo.InsertRange(tmp);
                    if (respone)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<ICollection<DiseasesRespone>> SearchDiseases(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim().ToLower();
                if(str.Length >= 2)
                {
                    var diseases = await _repo.GetDbSet().Where(x => x.DiseaseId.ToLower().Contains(str) || x.Name.ToLower().Contains(str)).ToListAsync();
                    if (diseases.Any())
                    {
                        var respone = new List<DiseasesRespone>();
                        var tmp = diseases.GroupBy(x => x.Code);
                        foreach(var code in tmp)
                        {
                            var twos = await _repo.GetDbSet().Where(x => x.Code.Equals(code.Key) && x.Number == null).ToListAsync();
                            foreach(var two in twos)
                            {
                                var tmp2 = code.Where(x => x.Number >= two.Start && x.Number <= two.End)
                                    .Select(x => new DiseasesRespone.DiseaseLevelThree()
                                    {
                                        DiseaseLevelThreeId = x.DiseaseId,
                                        DiseaseLevelThreeName = x.Name
                                    }).ToList();
                                if (tmp2.Any())
                                {
                                    var tmp3 = new DiseasesRespone()
                                    {
                                        DiseaseLevelTwoId = two.DiseaseId,
                                        DiseaseLevelTwoName = two.Name,
                                        DiseaseLevelThrees = tmp2
                                    };
                                    respone.Add(tmp3);
                                }
                            }
                        }
                        /*
                        
                        foreach (var two in diseases.Where(x => x.Number == null))
                        {                           
                            var tmp = new DiseasesRespone()
                            {
                                DiseaseLevelTwoId = two.DiseaseId,
                                DiseaseLevelTwoName = two.Name,
                                DiseaseLevelThrees = diseases.Where(x => x.Number >= two.Start && x.Number <= two.End && x.Code.Equals(two.Code)).Select(x => new DiseasesRespone.DiseaseLevelThree()
                                {
                                    DiseaseLevelThreeId = x.DiseaseId,
                                    DiseaseLevelThreeName = x.Name
                                }).ToList()
                            };
                            respone.Add(tmp);
                        }
                        */
                        if (respone.Any())
                        {
                            return respone;
                        }                       
                    }                   
                }
            }
            return null;
        }
    }
}
