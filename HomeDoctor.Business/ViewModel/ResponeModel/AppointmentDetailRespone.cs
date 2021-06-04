using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class AppointmentDetailRespone
    {
        public int PatientId { get; set; }
        public string FullNamePatient { get; set; }
        public string FullNameDoctor { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Diagnose { get; set; }
        public DateTime DateExamination { get; set; }
        public string ReasonCanceled { get; set; }
        public DateTime? DateCanceled { get; set; }
        public ICollection<MedicalInstruction>? MedicalInstructions { get; set; }
        public class MedicalInstruction
        {
            public int MedicalInstructionId { get; set; }
            public string MedicalInstructionType { get; set; }
            public DateTime DateCreated { get; set; }
        }
    }
   
}
