using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class HealthRecordOverviewRespone
    {
        // Patient
        public string FullNamePatient { get; set; }
        public string PhoneNumberPatient { get; set; }
        public string AddressPatient { get; set; }
        public string Career { get; set; }
        public DateTime DOBPatient { get; set; }
        public string Gender { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        // Account of patient and doctor
        public int AccountPatientId { get; set; }
        // Personal HR
        public string PersonalMedicalHistory { get; set; }
        public string FamilyMedicalHistory { get; set; }
        // Contract 
        public ICollection<Disease> Diseases { get; set; }
        public class Disease
        {
            public string DiseaseId { get; set; }
            public string DiseaseName { get; set; }
        }
    }
}
