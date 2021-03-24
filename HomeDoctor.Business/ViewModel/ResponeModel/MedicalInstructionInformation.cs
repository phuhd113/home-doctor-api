using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicalInstructionInformation
    {
        public int MedicalInstructionId { get; set; }
        public string MedicalInstructionType { get; set; }
        public DateTime DateCreate { get; set; }
        public string PatientFullName { get; set; }
        public string? Image { get; set; }
        public string Description { get; set; }     
        public string Diagnose { get; set; }
        public string PlaceHealthRecord { get; set; }
        public PrescriptionRespone? PrescriptionRespone { get; set; }
     
    }
}
