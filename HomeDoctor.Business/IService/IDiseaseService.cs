using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IDiseaseService
    {
        public Task<ICollection<Disease>> GetDiseases(string? status);
        public Task<Disease> GetDiseaseById(string diseaseId);
    }
}
