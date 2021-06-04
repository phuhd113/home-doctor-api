using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class PatientTrackingRespone
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }       
        public DateTime? AppointmentLast { get; set; }       
        public int HealthRecordId { get; set; }
        public int ContractId { get; set; }
        public string ContractStatus { get; set; }
        public int AccountPatientId { get; set; }     
        public string PersonalStatus { get; set; }
        public DateTime? DateUpdateStatus { get; set; }
        public bool? VitalSignScheduleFirst { get; set; }
        public bool? AppointmentFirst { get; set; }
        public bool SmartWatchConnected { get; set; }
        public ICollection<Disease> DiseaseContract { get; set; }
        public class Disease
        {
            public string DiseaseId { get; set; }
            public string DiseaseName { get; set; }
        }
    }
}
