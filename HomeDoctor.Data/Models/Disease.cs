using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Disease
    {
        public string DiseaseId {get;set;}
        public string Code { get; set; }
        public int? Number { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public string Name {get;set;}

        public ICollection<Contract> Contracts { get; set; }
        public ICollection<HealthRecord> HealthRecords { get; set; }

        public override string ToString()
        {
            return DiseaseId + " : "+Name;
        }
    }
}
