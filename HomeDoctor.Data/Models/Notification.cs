using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public bool Status { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreate { get; set; }
        public bool OnSystem { get; set; }
        // Relationship
        public int? AppointmentId { get; set; }
        // Contract
        public int? ContractId { get; set; }
        // MedicalInstruction
        public int? MedicalInstructionId { get; set; }
        public int? VitalSignShareId { get; set; }
        // Account Send
        public int? AccountSendId { get; set; }
        public string? BodyCustom { get; set; }
        // Account
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType {get;set;}
    }
}
