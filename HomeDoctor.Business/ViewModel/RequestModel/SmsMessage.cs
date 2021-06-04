using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class SmsMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
    }
}
