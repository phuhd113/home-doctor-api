using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeDoctor.Business.ViewModel.RequestModel
{
    public class OderPaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }
        public string BankCode { get; set; }
    }
}
