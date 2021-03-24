using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class PrescriptionRespone    {
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public DateTime? DateCanceled { get; set; }
        public string Status { get; set; }
        public string ReasonCancel { get; set; }
        public ICollection<MedicationScheduleRespone> MedicationSchedules { get; set; }

    }
}
