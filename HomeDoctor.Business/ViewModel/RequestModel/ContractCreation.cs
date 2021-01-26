using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class ContractCreation
    {
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public int PatientId { get; set; }
        public string Reason { get; set; }
        [Required]
        public DateTime DateStarted { get; set; }
        [Required]
        public int DaysOfTracking { get; set; }
    }
}
