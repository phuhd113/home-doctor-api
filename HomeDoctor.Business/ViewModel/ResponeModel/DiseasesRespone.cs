using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class DiseasesRespone
    {
        public string DiseaseLevelTwoId { get; set; }
        public string DiseaseLevelTwoName { get; set; }
        public ICollection<DiseaseLevelThree> DiseaseLevelThrees { get; set; }
        public class DiseaseLevelThree
        {
            public string DiseaseLevelThreeId { get; set; }
            public string DiseaseLevelThreeName { get; set; }
        } 
    }
}
