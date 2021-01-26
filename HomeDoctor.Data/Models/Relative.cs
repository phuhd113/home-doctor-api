using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Relative
    {
        public int RelativeId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
