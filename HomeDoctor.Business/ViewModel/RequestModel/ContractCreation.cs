using HomeDoctor.Data.Models;
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
        [Required]
        public DateTime DateStarted { get; set; }
        [Required]
        public int LicenseId { get; set; }
        public ICollection<string> DiseaseIds { get; set; }
        public string? Note { get; set; }
    }
}
