using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class ActionEveryDay
    {
        public int ActionEveryDayId { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Examination { get; set; }     
    }
}
