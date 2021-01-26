using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel
{
    public class PatientInformation
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Career { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public ICollection<RelativeInformation>? Relatives { get; set; }

    }
}
