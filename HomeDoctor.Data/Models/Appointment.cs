using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Appointment
    { 
        public int AppointmentId { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Status { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Note { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Diagnose { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateExamination { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ReasonCanceled { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCanceled { get; set; }
        // Relationship
        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public int HealthRecordId { get; set; }
        public HealthRecord HealthRecord { get; set; }


    }
}
