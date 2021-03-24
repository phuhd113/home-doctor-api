using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class MedicalInstruction
    {
        public int MedicalInstructionId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Diagnose { get; set; }
        public DateTime DateCreate { get; set; }       
        // Relationship
        public int MedicalInstructionTypeId { get; set; }
        public MedicalInstructionType MedicalInstructionType { get; set; }
        public int HealthRecordId { get; set; }
        public HealthRecord HealthRecord { get; set; }
        public Prescription Prescription { get; set; }
        public ICollection<MedicalInstructionShare> MedicalInstructionShares { get; set; }
        public VitalSignSchedule VitalSignSchedule { get; set; } 

    }
}
