using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class PersonalHealthRecord
    {
        [Key]
        public int PersonalHealthRecordId { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string PersonalMedicalHistory { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string FamilyMedicalHistory { get; set; }
        public bool SmartWatchConnected { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string? PersonalStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateUpdateStatus { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public ICollection<HealthRecord> HealthRecords { get; set; }
        public ICollection<VitalSignValue> VitalSignValues { get; set; }
        
    }
}
