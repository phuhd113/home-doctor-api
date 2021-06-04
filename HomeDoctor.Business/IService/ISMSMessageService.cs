using HomeDoctor.Business.ViewModel.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface ISMSMessageService
    {
        public Task SendSMSMessage(int doctorAccountId, int patientAccountId);

        public Task<string> SendCodeToVerify(string phoneNumber, string fullName);
        public Task<bool> ConfirmCode(string verificationCode);
    }
}
