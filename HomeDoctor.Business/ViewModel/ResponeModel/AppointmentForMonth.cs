using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class AppointmentForMonth
    {
        public string DateExamination { get; set; }      
        public ICollection<Appointment> Appointments { get; set; }
        public class Appointment
        {
            public int AppointmentId { get; set; }
            public string FullNamePatient { get; set; }
            public string FullNameDoctor { get; set; }
            public string Status { get; set; }
            public string Note { get; set; }
            public DateTime DateExamination { get; set; }
            public string ReasonCanceled { get; set; }
            public DateTime? DateCanceled { get; set; }
            public ICollection<MedicalInstruction> MedicalInstructions { get; set; }
        }
        public class MedicalInstruction
        {
            public int MedicalInstructionId { get; set; }
            public string MedicalInstructionType { get; set; }
        }
        
    }
}
