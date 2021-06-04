using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IFirebaseFCMService
    {
        public Task<bool> SaveToken(int patientId,string token);
        public Task PushNotification(int deviceType,int senderAccountID ,int accountId, int notiTypeId, int? contractId, int? medicalInstructionId,int? appointmentId,string? b);
    }
}
