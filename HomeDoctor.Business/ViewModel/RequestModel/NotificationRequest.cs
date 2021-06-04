using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class NotificationRequest
    {
        public int? ContractId { get; set; }
        public int? MedicalInstructionId { get; set; }
        public int? AppointmentId { get; set; }
        public int? AccountSendId { get; set; }
        public int AccountId { get; set; }
        public int NotificationTypeId { get; set; }
        public bool OnSystem { get; set; }
        public string? bodyCustom { get; set; }
        public int? VitalSignValueShareId { get; set; }
    }
}
