using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
