using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Disease
    {
        public string DiseaseId {get;set;}
        public string Name {get;set;}
        public string Status { get; set; }

        public ICollection<Contract>? Contracts { get; set; }

        public override string ToString()
        {
            return DiseaseId + " : "+Name;
        }
    }
}
