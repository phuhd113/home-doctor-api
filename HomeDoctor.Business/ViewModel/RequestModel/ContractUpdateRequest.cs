using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class ContractUpdateRequest
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Status { get; set; }
        public DateTime? DateStart { get; set; }
        public int? DaysOfTracking { get; set; }
        public ICollection<int> MedicalInstructionChooses { get; set; }       
    }
}
