using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class VitalSignValueShareRespone
    {
        public int VitalSignValueShareId { get; set; }
        public int HealthRecordId { get; set; }
        public string TimeShare { get; set; }
        public int MinuteShare { get; set; }
    }
}
