using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class VitalSignValueRequest
    {       
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int VitalSignTypeId { get; set; }
        [Required]
        public string TimeValue { get; set; }
        [Required]
        public string NumberValue { get; set; }
    }
}
