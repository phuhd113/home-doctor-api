﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class MedicalInstructionCreate
    {
        public int MedicalInstructionTypeId { get; set; }
        public int HealthRecordId { get; set; }
        public int PatientId { get; set; }
        public DateTime? DateTreatment { get; set; }
        public string? Description { get; set; }       
        public string? Conclusion { get; set; }
        public ICollection<string>? DiseaseIds { get; set; }
    }
}
