using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class MedicalInstruction
    {
        public int MedicalInstructionId { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Description { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Conclusion { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Status { get; set; }
        public int? MIShareFromId { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTreatment { get; set; }
        // Relationship
        public int MedicalInstructionTypeId { get; set; }
        public MedicalInstructionType MedicalInstructionType { get; set; }
        public int HealthRecordId { get; set; }
        public HealthRecord HealthRecord { get; set; }
        public int? PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
        public int? VitalSignScheduleId { get; set; }
        public VitalSignSchedule VitalSignSchedule { get; set; } 
        public ICollection<MedicalInstructionImage>? MedicalInstructionImages { get; set; }
        public int? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<Disease> Diseases { get; set; }
        public ICollection<ContractMedicalInstruction> ContractMedicalInstructions { get; set; }
    }
}
