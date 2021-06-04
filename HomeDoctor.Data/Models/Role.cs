using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Role
    { 
        public int RoleId { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string RoleName { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
