using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class MIPresciption
    {   
        public int HealthRecordId { get; set; }
        public int DoctorAccountId { get; set; }
        public string? Conclusion { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public string? Description { get; set; }    
        public int? AppointmentId { get; set; }
        public ICollection<string>? DiseaseIds { get; set; }
        public ICollection<MedicationSchedule>? MedicationScheduleCreates { get; set; }

        public class MedicationSchedule
        {
            public string MedicationName { get; set; }
            public string Content { get; set; }
            public string Unit { get; set; }
            public string UseTime { get; set; }
            public int Morning { get; set; }
            public int Noon { get; set; }
            public int AfterNoon { get; set; }
            public int Night { get; set; }
        }

    }
}
