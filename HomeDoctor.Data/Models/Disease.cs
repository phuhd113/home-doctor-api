using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Disease
    {
        public string DiseaseId {get;set;}
        [Required]
        [Column(TypeName = "varchar(2)")]
        public string Code { get; set; }
        public int? Number { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Name {get;set;}
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<HealthRecord> HealthRecords { get; set; }
        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public override string ToString()
        {
            return DiseaseId + " : "+Name;
        }
    }
}
