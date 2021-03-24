using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class MedicalInstructionShare
    {
        public int MedicalInstructionShareId { get; set; }
        public string Status { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public int MedicalInstructionId { get; set; }
        public MedicalInstruction MedicalInstruction { get; set; }
    }
}
