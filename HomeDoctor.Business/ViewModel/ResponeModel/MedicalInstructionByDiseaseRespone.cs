using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicalInstructionByDiseaseRespone
    {
        public string HealthRecordPlace { get; set; }
        public string DateCreate { get; set; }
        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public class MedicalInstruction
        {
            public int MedicalInstructionId { get; set; }
            public string Image { get; set; }
            public string DateCreate { get; set; }
        }
    }
}
