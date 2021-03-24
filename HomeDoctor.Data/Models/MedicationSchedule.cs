using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class MedicationSchedule
    {
        public int MedicationScheduleId { get; set; }
        public string MedicationName { get; set; }
        public string Content { get; set; }
        public string UseTime { get; set; }
        public string Unit { get; set; }       
        public int Morning { get; set; }
        public int Noon { get; set; }
        public int AfterNoon { get; set; }
        public int Night { get; set; }       
        //Relationship
        public int PrescriptionId { get; set; }
        public Prescription Prescription {get;set;}

    }
}
