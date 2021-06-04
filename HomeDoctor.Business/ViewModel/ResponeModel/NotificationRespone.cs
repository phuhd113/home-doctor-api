using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class NotificationRespone
    {
        public string DateCreate { get; set; }    
        public ICollection<Notification> Notifications { get; set; }
        public class Notification
        {
            public int NotificationId { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public bool Status { get; set; }
            public int NotificationType { get; set; }
            public int? ContractId { get; set; }
            public int? MedicalInstructionId { get; set; }
            public int? AppointmentId { get; set; }
            public double TimeAgo { get; set; }
            public string DateCreated { get; set; }
        }

    }
}
