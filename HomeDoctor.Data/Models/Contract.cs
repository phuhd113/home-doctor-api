﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Contract
    {
        public int ContractId { get; set; }      
        public string FullNameDoctor { get; set; }
        public string PhoneNumberDoctor { get; set; }
        public string WorkLocationDoctor { get; set; }
        public DateTime DOBDoctor { get; set; }
        public string FullNamePatient { get; set; }
        public string PhoneNumberPatient { get; set; }
        public string AddressPatient { get; set; }
        public DateTime DOBPatient { get; set; }

        public string ContractCode { get; set; }
        public string Reason { get; set; }       
        public string Status { get; set; }       
        public int DaysOfTracking { get; set; }
        public DateTime DateCreated { get; set; }              
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
