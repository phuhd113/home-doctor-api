using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class ContractInformation
    {
        public int ContractId { get; set; }
        // doctor
        public string FullNameDoctor { get; set; }
        public string PhoneNumberDoctor { get; set; }
        //patient
        public string FullNamePatient { get; set; }
        public string PhoneNumberPatient { get; set; }
        //contract
        public string ContractCode { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        //Lisence
        public string NameLicense { get; set; }
        public float PriceLicense { get; set; }
        public int DaysOfTracking { get; set; }
        //Diseace
        public ICollection<Disease> Diseases { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; 
        }
    }
}
