using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class HealthRecordInformation
    {
        public int HealthRecordId { get; set; }
        public string Place { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateFinished { get; set; }
        public DateTime? DateStarted { get; set; }
        public string? Status { get; set; }
        public string? ContractStatus { get; set; }
        public int? ContractId { get; set; }
        public ICollection<Disease> Diseases { get; set; }

        public class Disease
        {
            public string DiseaseId { get; set; }
            public string DiseaseName { get; set; }
        }


    }
}
