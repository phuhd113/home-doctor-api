using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class ContractDetailInformation
    {
        // doctor
        public int AccountDoctorId { get; set; }
        public int DoctorId { get; set; }
        public string FullNameDoctor { get; set; }
        public string PhoneNumberDoctor { get; set; }
        public string AddressDoctor { get; set; }
        public DateTime DOBDoctor { get; set; }
        public string WorkLocationDoctor { get; set; }
        public string Experience { get; set; }
        public string Specialization { get; set; }
        //patient
        public int AccountPatientId { get; set; }
        public int PatientId { get; set; }
        public string FullNamePatient { get; set; }
        public string PhoneNumberPatient { get; set; }
        public string AddressPatient { get; set; }
        public string GenderPatient { get; set; }
        public DateTime DOBPatient { get; set; }
        //contract
        public string ContractCode { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        //Lisence
        public string NameLicense { get; set; }
        public float PriceLicense { get; set; }
        public int DaysOfTracking { get; set; }
        //Diseace
        public ICollection<Disease> Diseases { get; set; }
        // MedicalInstructionShare 
        public ICollection<MedicalInstructionType> MedicalInstructionTypes { get; set; }
        //Time
        public DateTime DateCreated { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public class Disease
        {
            public string DiseaseId { get; set; }
            public string NameDisease { get; set; }
        }
        public class MedicalInstructionType
        {
            public string MedicalInstructionTypeName { get; set; }
            public ICollection<MedicalInstruction> MedicalInstructions { get; set; }

        }
        public class MedicalInstruction
        {
            public string Image { get; set; }
            public string Diagnose { get; set; }
            public string Description { get; set; }
        }
    }
}
