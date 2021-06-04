using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class MedicationSchedule
    {
        public int MedicationScheduleId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string MedicationName { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Content { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string UseTime { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string Unit { get; set; }       
        public int? Morning { get; set; }
        public int? Noon { get; set; }
        public int? AfterNoon { get; set; }
        public int? Night { get; set; }       
        //Relationship
        public int PrescriptionId { get; set; }
        public Prescription Prescription {get;set;}

    }
}
