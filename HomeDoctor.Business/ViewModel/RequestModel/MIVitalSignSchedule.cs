using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class MIVitalSignSchedule
    {
        public int HealthRecordId { get; set; }
        public int DoctorAccountId { get; set; }
        public int ContractId { get; set; }
        public string? Diagnose { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public string? Description { get; set; }
        public ICollection<VitalSign>? VitalSigns { get; set; }       
    }
    public class VitalSign
    {
        public int VitalSignTypeId { get; set; }
        public int? NumberMax { get; set; }
        public int? NumberMin { get; set; }
        public int? MinuteDangerInterval { get; set; }
        public string? TimeStart { get; set; }
        public int? MinuteAgain { get; set; }
    }
}
