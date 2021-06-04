using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class VitalSignValueShare
    {
        public int VitalSignValueShareId { get; set; }
        public int HealthRecordId { get; set; }
        public DateTime TimeShare { get; set; }
        public int MinuteShare { get; set; }
        //Relative 
        public HealthRecord HealthRecord { get; set; }
        
    }
}
