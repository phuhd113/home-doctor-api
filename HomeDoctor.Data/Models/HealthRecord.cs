using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public int PersonalHealthRecordId { get; set; }
        public PersonalHealthRecord PersonalHealthRecord { get; set; }
        public int? ContractId { get; set; }
        public Contract Contract { get; set; }
        public ICollection<Disease>? Diseases { get; set; } 
    }
}
