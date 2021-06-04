using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class VitalSignValue
    {
        public int VitalSignValueId { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        public string TimeValue { get; set; }
        public string NumberValue { get; set; }
        //Relationship
        public int PersonalHealthRecordId { get; set; }
        public PersonalHealthRecord PersonalHealthRecord { get; set; }
        public int VitalSignTypeId { get; set; }
        public VitalSignType VitalSignType { get; set; }
    }
}
