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
        public Contract ContractDetail { get; set; }
        // Disease
        public ICollection<Disease> Diseases { get; set; }
        // MedicalInstruction doctor choosed when create contract
        public ICollection<MedicalInstruction>? MedicalInstructions { get; set; }
        // Appointment 
        public Appointment? AppointmentNext { get; set; }
        // ConnectSmartWatch 
        public bool SmartWatchConnected { get; set; }
        public class Disease
        {
            public string DiseaseId { get; set; }
            public string DiseaseName { get; set; }
        }
        public class Appointment
        {
            public int AppointmentId { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
            public string Note { get; set; }
            public DateTime DateExamination { get; set; }
        }
        public class MedicalInstruction
        {
            public int MedicalInstructionId { get; set; }
            public string Conclusion { get; set; }
            public string MedicalInstructionType { get; set; }
            public int? PrescriptionId { get; set; }
            public int? VitalSignScheduleId { get; set; }
            public ICollection<string>? Images { get; set; }
        }       
        public class Contract
        {
            public int ContractId { get; set; }
            public string DateStarted { get; set; }
            public string DateFinished { get; set; }
        }
    }
}
