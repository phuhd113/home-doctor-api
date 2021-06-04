using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeDoctor.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _serPay;
        private readonly IContractService _serContract;
        private readonly IDoctorService _serDoctor;
        private readonly IPatientService _serPatient;
        private readonly INotificationService _serNotification;
        private readonly IFirebaseFCMService _serFirebase;

        public PaymentsController(IPaymentService serPay, IContractService serContract, IDoctorService serDoctor, IPatientService serPatient, INotificationService serNotification, IFirebaseFCMService serFirebase)
        {
            _serPay = serPay;
            _serContract = serContract;
            _serDoctor = serDoctor;
            _serPatient = serPatient;
            _serNotification = serNotification;
            _serFirebase = serFirebase;
        }

        [HttpGet("GetURLPayment")]
        public async Task<IActionResult> GetURLPayment([FromQuery]OderPaymentRequest order)
        {
            if(order != null)
            {
                var respone = await _serPay.PayContract(order);
                if (!string.IsNullOrEmpty(respone))
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        
        [HttpPost("CheckPaymentStatus")]
        public async Task<IActionResult> CheckPaymentStatus(int contractId,[FromBody]string urlRespone)
        {
            if (!string.IsNullOrEmpty(urlRespone))
            {
                var respone = await _serPay.CheckPaymentStatus(contractId,urlRespone);
                if(respone != null)
                {
                    JObject jObject = JObject.Parse(respone);
                    var rspCode = (string)jObject["RspCode"];
                    if (rspCode.Equals("00"))
                    {
                        
                        var check = await _serContract.UpdateStatusContract(contractId, null, null, "SIGNED", null);
                        if (check)
                        {
                            var contract = await _serContract.GetContractByContractId(contractId);
                            if(contract != null)
                            {
                                // Save notification of patient for doctor
                                var notiRequest = new NotificationRequest()
                                {
                                    AccountSendId = contract.AccountPatientId,
                                    AccountId = contract.AccountDoctorId,
                                    ContractId = contractId,
                                    NotificationTypeId = 9
                                };
                                await _serNotification.InsertNotification(notiRequest);
                                // Firebase Notification for doctor
                                await _serFirebase.PushNotification(2, contract.AccountPatientId, contract.AccountDoctorId, 9, contractId, null, null, null);
                            }
                        }
                    }
                    return Ok(respone);
                }
            }
            return NotFound();
        }
    }
}
