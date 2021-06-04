using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class VitalSignScheduleRespone
    {
        public int MedicalInstructionId { get; set; }
        public int VitalSignScheduleId { get; set; }
        public int PatientAccountId { get; set; }
        public int DoctorAccountId { get; set; }
        public string Status { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateCanceled { get; set; }
        public ICollection<VitalSign> VitalSigns { get; set; }
        public class VitalSign
        {
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
