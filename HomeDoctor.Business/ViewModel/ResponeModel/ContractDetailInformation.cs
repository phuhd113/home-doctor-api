using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class ContractDetailInformation
    {
        // doctor
        public int ContractId { get; set; }
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
        public decimal PriceLicense { get; set; }
        public int DaysOfTracking { get; set; }
        //Diseace
        public ICollection<string> DiseaseContracts { get; set; }
        // MedicalInstructionShare 
        public ICollection<MedicalInstructionDisease> MedicalInstructionDiseases { get; set; }
        public ICollection<MedicalInstruction> MedicalInstructionOthers { get; set; }
        public ICollection<MedicalInstruction> MedicalInstructionChoosed { get; set; }

      
        //Time
        public DateTime DateCreated { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public class MedicalInstructionDisease
        {
            public string DiseaseId { get; set; }
            public string NameDisease { get; set; }
            public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        }       
        public class MedicalInstruction
        {
            public int MedicalInstructionId { get; set; }
            public string MedicalInstructionTypeName { get; set; }
            public int? MIShareFromId { get; set; }
            public ICollection<string>? Images { get; set; }
            public string Conclusion { get; set; }
            public string Description { get; set; }
            public DateTime? DateCreated { get; set; }
            public ICollection<string> Diseases { get; set; }
        }
    }
}
