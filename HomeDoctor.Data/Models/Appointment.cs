using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Appointment
    { 
        public int AppointmentId { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExamination { get; set; }        
        public string? ReasonCanceled { get; set; }
        public DateTime? DateCanceled { get; set; }

        // Relationship
        public int ContractId { get; set; }
        public Contract Contract { get; set; }


    }
}
