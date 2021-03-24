using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
     public class HealthRecordCreate
    {
        public int PatientId { get; set; }
        public ICollection<string> DiceaseIds { get; set; } 
        public string Place { get; set; } 
        public string? Description { get; set; }        
    }
}
