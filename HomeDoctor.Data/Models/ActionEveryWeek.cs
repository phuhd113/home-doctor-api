using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class ActionEveryWeek
    {
        public int ActionEveryWeekId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool AppointmentWeek { get; set; }
        public bool PrescriptionWeek { get; set; }
        public bool VitalSignWeek { get; set; }
        public ICollection<ActionEveryDay> ActionEveryDays { get; set; }
    }
}
