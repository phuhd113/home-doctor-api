using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class VitalSignValuePatientIdRespone
    {
        public DateTime VitalSignValueDateCreated { get; set; }
        public ICollection<VitalSign> VitalSigns { get; set; }
        public ICollection<VitalSignValue> VitalSignValues { get; set; }
        public class VitalSign
        {
            public string VitalSignType { get; set; }
            public int VitalSignTypeId { get; set; }
            public int? NumberMax { get; set; }
            public int? NumberMin { get; set; }
            public int? MinuteDangerInterval { get; set; }
            public int? MinuteNormalInterval { get; set; }
            public DateTime? TimeStart { get; set; }
            public int? MinuteAgain { get; set; }
        }
        public class VitalSignValue
        {
            public int VitalSignTypeId { get; set; }
            public DateTime DateCreated { get; set; }
            public string TimeValue { get; set; }
            public string NumberValue { get; set; }
        }
    }
}
