using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Career { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public ICollection<Relative>? Relatives { get; set; }
        public PersonalHealthRecord PersonalHealthRecord { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
