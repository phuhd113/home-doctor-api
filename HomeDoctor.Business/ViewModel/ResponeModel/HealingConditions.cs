using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class HealingConditions
    {
        public bool SmartWatchConnected { get; set; }
        public ICollection<MedicalInstructionType> MedicalInstructionTypes { get; set; }

        public class MedicalInstructionType
        {
            public string MIType { get; set; }
            public ICollection<MedicalInstructionShare> MedicalInstructions { get; set; }
        }
        public class MedicalInstructionShare
        {
            public int MedicalInstructionId { get; set; }
            public string Diagnose { get; set; }
            public ICollection<string> Images { get; set; }
        }
    }
}
