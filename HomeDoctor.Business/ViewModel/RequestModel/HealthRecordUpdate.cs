using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class HealthRecordUpdate
    {
        public string Place { get; set; }
        public string Description { get; set; }
        public ICollection<string>? Diseases { get; set; }
    }
}
