using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public int Weight { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Career { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public ICollection<Relative>? Relatives { get; set; }
        public PersonalHealthRecord PersonalHealthRecord { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
