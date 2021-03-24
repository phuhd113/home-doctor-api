using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public string Status { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public DateTime? DateCanceled { get; set; }
        public string? ReasonCancel { get; set; }

        //Relationship
        public int MedicalInstructionId { get; set; }
        public MedicalInstruction MedicalInstruction { get; set; }
        public ICollection<MedicationSchedule> MedicationSchedules { get; set; }
    }
}
