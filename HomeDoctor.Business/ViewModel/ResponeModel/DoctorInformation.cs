using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class DoctorInformation
    {
        public int DoctorId { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string WorkLocation { get; set; }
        public string Experience { get; set; }
        public string Specialization { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
