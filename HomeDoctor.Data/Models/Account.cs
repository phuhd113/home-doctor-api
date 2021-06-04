using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Account
    {
        public int AccountId { get;set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Username { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Password { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FullName { get;set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get;set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get;set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Address { get;set; }
        [Column(TypeName = "varchar(255)")]
        public string FireBaseToken { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Column(TypeName = "varchar(7)")]
        public string Gender { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
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
