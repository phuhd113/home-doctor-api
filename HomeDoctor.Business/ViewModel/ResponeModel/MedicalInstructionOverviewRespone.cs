using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicalInstructionOverviewRespone
    {
        public int MedicalInstructionId { get; set; }
        public int MedicalInstructionTypeId { get; set; }
        public string MedicalInstructionTypeName { get; set; }
        public string Description { get; set; }
        public string Conclusion { get; set; }
        public ICollection<string>? Images { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateTreatment { get; set; }
        public string Status { get; set; }
        public ICollection<string> Diseases { get; set; }
    }
}
