using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicalInstructionShareRespone
    {
        public string MedicalInstructionType { get; set; }
        public ICollection<MedicalInstructionInformation> MedicalInstructions { get; set; }

        public class MedicalInstructionInformation
        {
            public string Status { get; set; }
            public int MedicalInstructionId { get; set; }
            public string Diagnose { get; set; }
            public string Description { get; set; }
            public ICollection<string?> Images { get; set; }
            public PrescriptionRespone? PrescriptionRespone { get; set; }
        }
    }
}
