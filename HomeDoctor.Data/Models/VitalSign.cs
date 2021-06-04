using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class VitalSign
    {
        public int VitalSignId { get; set; }
        public int? NumberMax { get; set; }
        public int? NumberMin { get; set; }
        public int? MinuteDangerInterval { get; set; }
        public int? MinuteNormalInterval { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TimeStart { get; set; }
        public int? MinuteAgain { get; set; }
        // Relationship
        public int VitalSignTypeId { get; set; }
        public VitalSignType VitalSignType { get; set; }
        public int VitalSignScheduleId { get; set; }
        public VitalSignSchedule VitalSignSchedule { get; set; }

    }
}
