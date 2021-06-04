using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class AppointmentCreate
    {
        [Required]
        public int HealthRecordId { get; set; }
        public int AccountDoctorId { get; set; }
        public int AccountPatientId { get; set; }
        [Required]
        public DateTime DateExamination { get; set; }
        public string Note { get; set; }       
    }
}
