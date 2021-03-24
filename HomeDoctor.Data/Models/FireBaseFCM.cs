using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class FireBaseFCM
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int AccountId { get; set; }
        public Account Account {get;set;}
    }
}
