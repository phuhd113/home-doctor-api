using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class ContractInformation
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ContractCode { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public int DaysOfTracking { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
