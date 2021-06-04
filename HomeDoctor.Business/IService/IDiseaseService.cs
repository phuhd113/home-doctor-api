using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IDiseaseService
    {
        public Task<ICollection<DiseasesRespone>> GetDiseases();
        public Task<DiseaseRespone> GetDiseaseById(string diseaseId);
        public Task<bool> InsertDisease(string diseaseId, string code, int? number, int? start, int? end, string name);

        public Task<ICollection<DiseasesRespone>> GetHeartDiseases();

        public Task<ICollection<DiseasesRespone>> GetDiseasesToCreateContract(int patientId);
        public Task<bool> InsertDiseases(ICollection<DiseaseCreate> diseases);

        public Task<ICollection<DiseasesRespone>> SearchDiseases(string str);

    }
}
