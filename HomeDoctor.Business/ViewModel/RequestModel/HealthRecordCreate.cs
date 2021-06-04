using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
     public class HealthRecordCreate
    {
        public int PatientId { get; set; }
        public ICollection<string> DiseaseIds { get; set; } 
        public string Place { get; set; } 
        public string? Description { get; set; }        
        public DateTime? DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }
    }
}
