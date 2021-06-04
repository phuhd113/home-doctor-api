using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class DiseaseCreate
    {
        public string DiseaseId { get; set; }
        public string Code { get; set; }
        public int? Number { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public string Name { get; set; }
    }
}
