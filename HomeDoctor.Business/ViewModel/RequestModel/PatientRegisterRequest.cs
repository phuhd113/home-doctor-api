using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class PatientRegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public PersonalInformation PatientInformation { get; set; }
        public class PersonalInformation
        {
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string? Email { get; set; }
            public string Address { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Gender { get; set; }
            public string? Career { get; set; }
            public PersonalHealthRecord PatientHealthRecord { get; set; }            
        }
        public class PersonalHealthRecord
        {
            public int Height { get; set; }
            public int Weight { get; set; }
            public string? PersonalMedicalHistory { get; set; }
            public string? FamilyMedicalHistory { get; set; }
            public ICollection<Relative> Relatives { get; set; }
            public class Relative
            {
                public string FullNameRelative { get; set; }
                public string PhoneNumber { get; set; }
            }
        }
    }
}
