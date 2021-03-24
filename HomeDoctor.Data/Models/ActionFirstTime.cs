using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class ActionFirstTime
    {
        public int ActionFirstTimeId { get; set; }
        public bool PrescriptionFirst { get; set; }
        public bool AppointmentFirst { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<ActionEveryWeek> ActionEveryWeeks { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
    }
}
