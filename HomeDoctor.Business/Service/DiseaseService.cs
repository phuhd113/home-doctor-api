using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.Models;
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

        public async Task<Disease> GetDiseaseById(string diseaseId)
        {
            if (!string.IsNullOrEmpty(diseaseId))
            {
                var disease = _repo.GetDbSet().FirstOrDefault(x => x.DiseaseId.Equals(diseaseId));
                if (disease != null)
                {
                    return disease;
                }
            }
            return null;
        }

        public async Task<ICollection<Disease>> GetDiseases(string status)
        {
            var diseases = _repo.GetDbSet().Where(x => string.IsNullOrEmpty(status) ? true : x.Status.Equals(status.ToUpper()));
            if (diseases.Count() != 0)
            {
                return diseases.ToList();
            }
            return null;
        }
    }
}
