using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class MedicalInstructionImage
    {
        public int MedicalInstructionImageId { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Image { get; set; }
        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
    }
}
