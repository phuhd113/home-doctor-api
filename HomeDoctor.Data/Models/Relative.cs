using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Relative
    {
        public int RelativeId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string FullName { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
