using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicalInstructionInformation
    {
        public int MedicalInstructionId { get; set; }
        public int MedicalInstructionTypeId { get; set; }
        public string MedicalInstructionTypeName { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateTreatement { get; set; }
        public string PatientFullName { get; set; }
        public ICollection<string>? Images { get; set; }
        public string Description { get; set; }     
        public string Conclusion { get; set; }
        public string PlaceHealthRecord { get; set; }
        public string Status { get; set; }
        public int? AppointmentId { get; set; }
        public ICollection<string> Diseases { get; set; }
        public PrescriptionRespone? PrescriptionRespone { get; set; }    
        public VitalSignSchedule? VitalSignScheduleRespone { get; set; }
        
        public class VitalSignSchedule
        {
            public DateTime TimeStared { get; set; }
            public DateTime TimeCanceled { get; set; }
            public ICollection<VitalSign> VitalSigns { get; set; }

            public class VitalSign
            {
                public int VitalSignTypeId { get; set; }
                public string VitalSignType { get; set; }
                public int? NumberMax { get; set; }
                public int? NumberMin { get; set; }
                public int? MinuteDangerInterval { get; set; }
                public int? MinuteNormalInterval { get; set; }
                public DateTime? TimeStart { get; set; }
                public int? MinuteAgain { get; set; }
            }
        }      
    }
}
