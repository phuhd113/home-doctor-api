using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class PersonalHealthRecord
    {
        [Key]
        public int PersonalHealthRecordId { get; set; }
        public string PersonalMedicalHistory { get; set; }
        public string FamilyMedicalHistory { get; set; }
        public bool SmartWatchConnected { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public ICollection<HealthRecord> HealthRecords { get; set; }
        
    }
}
