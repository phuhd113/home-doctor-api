using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;

namespace HomeDoctor.Business.Service
{
    public class SMSMessageService : ISMSMessageService
    {
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _uow;
        private IConfiguration _configuration { get; }

        public SMSMessageService(IUnitOfWork uow, IConfiguration configuration)
        {
            _configuration = configuration;
            _uow = uow;
            _repoAccount = _uow.GetRepository<Account>();
        }

        public async Task SendSMSMessage(int doctorAccountId,int patientAccountId)
        {
            try
            {
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];                
                TwilioClient.Init(accountSid, authToken);
                if (doctorAccountId != 0 && patientAccountId != 0)
                {
                    var phoneDoctor = await _repoAccount.GetDbSet().Where(x => x.AccountId == doctorAccountId)
                        .Select(x => x.PhoneNumber).FirstOrDefaultAsync();
                    var patient = await _repoAccount.GetDbSet().Where(x => x.AccountId == patientAccountId)
                        .Select(x => new {
                            x.FullName,
                            Relatives = x.Patient.Relatives.Select(y => y.PhoneNumber).ToList(),
                            PhonePatient = x.PhoneNumber
                        }
                        ).FirstOrDefaultAsync();

                    // SMS to Doctor
                    if (!string.IsNullOrEmpty(phoneDoctor) && patient != null)
                    {
                        var phoneNumber = "+84" + phoneDoctor.Replace(".", "").Substring(1);
                        var body = "Bệnh nhân " + patient.FullName + " có trạng thái nguy hiểm. Yêu cầu bác sĩ kiểm tra sinh hiệu của bệnh nhân trên hệ thống và liên hệ ngay với bệnh nhân nếu cần thiết.SĐT bệnh nhân : " + patient.PhonePatient;
                        if (patient.Relatives.Any())
                        {
                            body = body + " ; SĐT người nhà : " + patient.Relatives[0];
                        }
                        var message = await MessageResource.CreateAsync(
                            body: body,
                            from: new Twilio.Types.PhoneNumber("+13125258851"),
                            to: new Twilio.Types.PhoneNumber(phoneNumber)
                        );
                    }

                    // SMS to Relative
                    if (patient.Relatives.Any())
                    {
                        foreach (var phoneRelative in patient.Relatives)
                        {
                            var phoneNumber = "+84" + phoneRelative.Replace(".", "").Substring(1);
                            var body = "Người thân " + patient.FullName + " có trạng thái nguy hiểm. Yêu cầu người nhà kiểm tra tình trạng của bệnh nhân và liên hệ ngay với bác sĩ nếu cần thiết. SĐT bác sĩ: " + phoneDoctor + " ;SĐT bệnh nhân : " + patient.PhonePatient;
                            var message = await MessageResource.CreateAsync(
                                body: body,
                                from: new Twilio.Types.PhoneNumber("+13125258851"),
                                to: new Twilio.Types.PhoneNumber(phoneNumber)
                            );
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        public async Task<string> SendCodeToVerify(string phoneNumber,string fullName)
        {
            try
            {
                //var verificationServiceSID = _configuration["Twilio:VerificationServiceSID"];
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                TwilioClient.Init(accountSid, authToken);
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    var validationRequest = await ValidationRequestResource.CreateAsync(
                        friendlyName: fullName,
                        phoneNumber : phoneNumber
                        
                        
                    );
                    if (!string.IsNullOrEmpty(validationRequest.ValidationCode))
                    {
                        return validationRequest.ValidationCode;
                    }
                }
                return null;
                    /*
                    var authToken = _configuration["Twilio:AuthToken"];
                    TwilioClient.Init(accountSid, authToken);
                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        var verification = await VerificationResource.CreateAsync(
                            to: phoneNumber,
                            channel: "sms",
                            pathServiceSid: verificationServiceSID
                            );
                        if(verification.Status == "pending")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    */
            }
            catch(Exception e)
            {              
                Console.WriteLine("SMSservice : " + e.ToString());
                return null;
            }
            
            
        }

        public async Task<bool> ConfirmCode(string verificationCode)
        {
            if (!string.IsNullOrEmpty(verificationCode))
            {

                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];

                TwilioClient.Init(accountSid, authToken);
                var outgoingCallerIds = OutgoingCallerIdResource.Read(limit: 20);

                foreach (var record in outgoingCallerIds)
                {
                    Console.WriteLine(record.Sid);
                }
                /*
                var outgoingCallerIds = OutgoingCallerIdResource.Read(                  
                    phoneNumber: new Twilio.Types.PhoneNumber("+14158675310"),
                    limit: 20         
                );
                */


            }
            /*
            if (!string.IsNullOrEmpty(verificationCode))
            {
                var verificationServiceSID = _configuration["Twilio:VerificationServiceSID"];
                var verification = await VerificationCheckResource.CreateAsync(
                     to: phoneNumber,
                     code: verificationCode,
                     pathServiceSid: verificationServiceSID
                 );
                if(verification.Status == "approved")
                {
                    var identityUser = await _userManager.GetUserAsync(User);
                    identityUser.PhoneNumberConfirmed = true;
                    var updateResult = await _userManager.UpdateAsync(identityUser);
                }
                else 
                { 
                    return false; 
                }
            }
            */
            return false;
        }
    }
}
