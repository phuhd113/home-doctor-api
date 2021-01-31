using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class License
    {
        public int LicenseId { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        

    }
}
