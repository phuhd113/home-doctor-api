using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class LicenseCreate
    {
        public int? FromBy { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
