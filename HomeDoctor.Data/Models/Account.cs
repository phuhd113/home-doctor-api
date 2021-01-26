using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Account
    {
        public int AccountId { get;set; }
        public string FullName { get;set; }       
        public string PhoneNumber { get;set; }
        public string Email { get;set; }
        public string Address { get;set; }
        public DateTime DateOfBirth { get; set; }     
        public string Gender { get; set; }
        public DateTime DateCreated { get; set; }
        public Role Role { get; set; }
        
    }
}
