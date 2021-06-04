using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class ContractMedicalInstruction
    {
        [Key, Column(Order = 0), ForeignKey("Contract")]
        public int ContractId { get; set; }
        [Key, Column(Order = 1), ForeignKey("MedicalInstruction")]
        public int MedicalInstructionId { get; set; }
        public Contract Contract { get; set; }
        public MedicalInstruction MedicalInstruction { get; set; }
        public string? DiseaseChoosedId { get; set; }
        public string DiseaseIds { get; set; }
        public string Description { get; set; }
        public string Conclusion { get; set; }        
    }
}
