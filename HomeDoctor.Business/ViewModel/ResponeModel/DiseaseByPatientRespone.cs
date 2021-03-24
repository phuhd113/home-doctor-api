using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.ResponeModel
{
    public class DiseaseByPatientRespone
    {
        public string DiseaseLevelTwoId { get; set; }
        public string DiseaseLeverTwoName { get; set; }
        public ICollection<DiseaseLeverThree> DiseaseLeverThrees { get; set; }
        public class DiseaseLeverThree
        {
            public string DiseaseLevelThreeId { get; set; }
            public string DiseaseLeverThreeName { get; set; }
        }
    }
}
