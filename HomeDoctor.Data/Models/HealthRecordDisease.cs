using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class HealthRecordDisease
    {
        public int HealthRecordId { get; set; }
        public HealthRecord HealthRecord { get; set; }
        public int DiseaseId { get; set; }
        public Disease Disease { get; set; }
        public bool IsMain { get; set; }
    }
}
