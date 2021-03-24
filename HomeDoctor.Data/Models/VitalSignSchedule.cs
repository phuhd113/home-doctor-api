using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class VitalSignSchedule
    {
        public int VitalSignScheduleId { get; set; }
        public string Status { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        //Relationship
        public int MedicalInstructionId { get; set; }
        public MedicalInstruction MedicalInstruction { get; set; }
        public ICollection<VitalSign> VitalSigns { get; set; }
    }
}
