using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class VitalSignSchedule
    {
        public int VitalSignScheduleId { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Status { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateStarted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCanceled { get; set; }
        //Relationship
        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public ICollection<VitalSign> VitalSigns { get; set; }
    }
}
