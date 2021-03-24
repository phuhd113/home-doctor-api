using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class MedicationScheduleRespone
    {
        public string MedicationName { get; set; }
        public string Content { get; set; }
        public string UseTime { get; set; }
        public string Unit { get; set; }
        public int Morning { get; set; }
        public int Noon { get; set; }
        public int AfterNoon { get; set; }
        public int Night { get; set; }
    }
}
