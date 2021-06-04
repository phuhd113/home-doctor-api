using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Status { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateStarted { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateFinished { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCanceled { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ReasonCancel { get; set; }
        //Relationship
        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public ICollection<MedicationSchedule> MedicationSchedules { get; set; }
    }
}
