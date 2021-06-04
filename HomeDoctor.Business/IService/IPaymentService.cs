using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IPaymentService
    {
        public Task<string> PayContract(OderPaymentRequest orderInfor);

        public Task<string> CheckPaymentStatus(int contractId,string urlRespone);
    }
}
