using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Place { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateStarted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateFinished { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string? Status { get; set; }
        public bool VitalSignScheduleFirst { get; set; }
        public bool AppointmentFirst { get; set; }

        //Relationship

        public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        public int PersonalHealthRecordId { get; set; }
        public PersonalHealthRecord PersonalHealthRecord { get; set; }
        public int? ContractId { get; set; }
        public Contract Contract { get; set; }
        public ICollection<Disease>? Diseases { get; set; } 
        public ICollection<Appointment> Appointments { get; set; }
         
    }
}
