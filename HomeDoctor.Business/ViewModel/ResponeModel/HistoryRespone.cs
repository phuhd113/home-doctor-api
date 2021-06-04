using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class HistoryRespone
    {
        public string DateCreate { get; set; }
        public ICollection<History> Histories { get; set; }
        public class History
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public int HistoryType { get; set; }
            public int? MedicalInstructionId { get; set; }
            public int? ContractId { get; set; }
            public double TimeAgo { get; set; }
        }
        

    }
}
