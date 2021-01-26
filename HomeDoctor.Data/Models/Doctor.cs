using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string WorkLocation { get; set; }
        public string Experience { get; set; }
        public string Specialization { get; set; }
        public string Details { get; set; }
        public Account Account { get; set; }

    }
}
