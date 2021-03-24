using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicalInstructionToShareRespone
    {
        public string HealthRecordPlace { get; set; }
        public ICollection<MedicalInstructionType> MedicalInstructionTypes { get; set; }

        public class MedicalInstructionType
        {
            public string MIType { get; set; }
            public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        }
        public class MedicalInstruction
        {
            public int MedicalInstructionId { get; set; }
            public string? Image { get; set; }
        }
    }
}
