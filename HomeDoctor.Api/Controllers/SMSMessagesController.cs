using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SMSMessagesController : ControllerBase
    {
        private readonly ISMSMessageService _serSMS;

        public SMSMessagesController(ISMSMessageService serSMS)
        {
            _serSMS = serSMS;
        }

        [HttpPost]
        public async Task<IActionResult> SendSms(int doctorAccountId, int patientAccountId)
        {
            if(doctorAccountId != 0 && patientAccountId != 0)
            {
                await _serSMS.SendSMSMessage(doctorAccountId, patientAccountId);
                return StatusCode(204);
            }

            return BadRequest();
        }

        [HttpPost("Test")]
        public async Task<IActionResult> SendCode(string phoneNumber,string fullName)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var respone = await _serSMS.SendCodeToVerify(phoneNumber,fullName);
                if (respone != null)
                {
                    return Ok(respone);
                }
            }
            return BadRequest();
        }
        [HttpPost("Test1")]
        public async Task<IActionResult> ConfirmCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                
                var respone = await _serSMS.ConfirmCode(code);
                if (respone)
                {
                    return StatusCode(201);
                }
                
            }
            return BadRequest();
        }
    }
}
