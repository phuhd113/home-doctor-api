using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int AccountId { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string WorkLocation { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Experience { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Specialization { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Details { get; set; }
        public Account Account { get; set; }

    }
}
