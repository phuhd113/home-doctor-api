using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class VitalSignType
    {
        public int VitalSignTypeId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string VitalSignName { get; set; }
    }
}
