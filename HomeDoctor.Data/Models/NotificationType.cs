using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class NotificationType
    {
        public int NotificationTypeId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Body { get; set; }
        
    }
}
