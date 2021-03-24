using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreate { get; set; }
        public bool OnSystem { get; set; }
        // Contract
        public int? ContractId { get; set; }
        public Contract Contract { get; set; }
        // MedicalInstruction
        public int? MedicalInstructionId { get; set; }
        public MedicalInstruction MedicalInstruction { get; set; }
        // Account Send
        public int? AccountSendId { get; set; }
        // Account
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType {get;set;}
    }
}
