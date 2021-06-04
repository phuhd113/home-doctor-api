using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicalInstructionByDiseaseRespone
    {
        public string HealthRecordPlace { get; set; }
        public string DateCreated { get; set; }
        public ICollection<string> Diseases { get; set; }
        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public class MedicalInstruction
        {
            public int MedicalInstructionId { get; set; }
            public string? Disease { get; set; }
            public ICollection<string>? Images { get; set; }
            public string DateTreatment { get; set; }
            public string Conclusion { get; set; }
        }       
    }
}
