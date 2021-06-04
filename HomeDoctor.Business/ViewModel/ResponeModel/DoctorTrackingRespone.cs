using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class DoctorTrackingRespone
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int HealthRecordId { get; set; }
        public int ContractId { get; set; }
        public int AccountDoctorId { get; set; }       
        public string DateContractStarted { get; set; }
    }
}
