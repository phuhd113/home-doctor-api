using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Account
    {
        public int AccountId { get;set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get;set; }       
        public string PhoneNumber { get;set; }
        public string Email { get;set; }
        public string Address { get;set; }
        public DateTime DateOfBirth { get; set; }     
        public string Gender { get; set; }
        public DateTime DateCreated { get; set; }
        //Role
        public int RoleId { get; set; }
        public Role Role { get; set; }
        //Patient
        public Patient? Patient { get; set; }
        //Doctor
        public Doctor? Doctor { get; set; }
        
    }
}
