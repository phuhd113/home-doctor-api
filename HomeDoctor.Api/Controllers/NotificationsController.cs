using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _serNoti;
        private readonly IFirebaseFCMService _serFB;
        private readonly IContractService _serContract;
        private readonly IMedicalInstructionTypeService _serMIT;

        public NotificationsController(INotificationService serNoti, IFirebaseFCMService serFB, IContractService serContract, IMedicalInstructionTypeService serMIT)
        {
            _serNoti = serNoti;
            _serFB = serFB;
            _serContract = serContract;
            _serMIT = serMIT;
        }




        /// <summary>
        /// Get list notification by accountId. Sort by DateCreate
        /// </summary>      
        [HttpGet]
        public async Task<IActionResult> GetNotificationByAccountId(int accountId)
        {
            if (accountId != 0)
            {
                var noti = await _serNoti.GetNotificationByAccountId(accountId, false);
                if (noti != null)
                {
                    return Ok(noti);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// Update status of notification when SEEN.
        /// </summary>      
        [HttpPut]
        public async Task<IActionResult> UpdateStatusNotificationByNotiId(int notiId)
        {
            if (notiId != 0)
            {
                var check = await _serNoti.UpdateStatusNotificationByNotiId(notiId);
                if (check)
                {
                    return StatusCode(204, "Seen");
                }
            }
            return BadRequest();
        }
        [HttpGet("GetHistoryByAccountId")]
        public async Task<IActionResult> GetHistoryByAccountId(int accountId)
        {
            if (accountId != 0)
            {
                var history = await _serNoti.GetHistoryByAccountId(accountId);
                if (history != null)
                {
                    return Ok(history);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> SendNotification(int deviceType, int notificationType, int senderAccountId, int recipientAccountId,string? bodyCustom)
        {
            if (notificationType != 0 && senderAccountId != 0 && recipientAccountId != 0)
            {
                var noti = new NotificationRequest()
                {
                    AccountSendId = senderAccountId,
                    AccountId = recipientAccountId,
                    NotificationTypeId = notificationType,
                    bodyCustom = bodyCustom
                };
                if (notificationType == 2 || notificationType == 17)
                {
                    noti.OnSystem = true;
                    // Send SMS when status DANGER
                    //await _serSMS.SendSMSMessage(recipientAccountId, senderAccountId);
                }
                var insertNoti = await _serNoti.InsertNotification(noti);
                // push notification firebase
                if (insertNoti)
                {
                    await _serFB.PushNotification(deviceType, senderAccountId, recipientAccountId, notificationType, null, null, null,bodyCustom);
                }
                if (insertNoti)
                {
                    return StatusCode(201);
                }

            }
            return BadRequest();
        }
        [HttpPost("SendNotiRequireMedicalInstruction")]
        public async Task<IActionResult> SendNotiRequireMedicalInstruction(int doctorAccountId, int patientAccountId, int medicalInstructionTypeId,int contractId,string? note)
        {
            if (doctorAccountId != 0 && patientAccountId != 0 && medicalInstructionTypeId != 0)
            {
                
                var noti = new NotificationRequest()
                {
                    AccountSendId = doctorAccountId,
                    AccountId = patientAccountId,
                    ContractId = contractId,
                    NotificationTypeId = 12,        
                    MedicalInstructionId = medicalInstructionTypeId
                };
                var mit = await _serMIT.GetMedicalInstructionTypeById(medicalInstructionTypeId);
                if(mit != null)
                {
                    noti.bodyCustom = mit.Name;
                    if (!string.IsNullOrEmpty(note))
                    {
                        noti.bodyCustom = mit.Name + " - " + note;
                    }
                }
                
                var insertNoti = await _serNoti.InsertNotification(noti);
                // push notification firebase
                if (insertNoti)
                {
                    await _serFB.PushNotification(1, doctorAccountId, patientAccountId, 12, contractId,medicalInstructionTypeId, null,noti.bodyCustom);
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }
        [HttpGet("GetSystemNotification")]
        public async Task<IActionResult> GetSystemNotificationByAccountId(int accountId)
        {
            if (accountId != 0)
            {
                var noti = await _serNoti.GetNotificationByAccountId(accountId, true);
                if (noti != null)
                {
                    return Ok(noti);
                }
            }
            return NotFound();
        }
        [HttpGet("GetTimeLinePatient")]
        public async Task<IActionResult> GetTimeLineOfPatient([Required]int accountPatientId, [Required]int accountDoctorID,[Required] DateTime dateTime)
        {
            if(accountPatientId != 0 && accountDoctorID != 0 && dateTime != null)
            {
                var respone = await _serNoti.GetTimeLineOfPatient(accountPatientId, accountDoctorID, dateTime);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }

        [HttpGet("GetDateTimeHaveNotification")]
        public async Task<IActionResult> GetDateTimeHaveNotification([Required] int accountPatientId, [Required] int accountDoctorID)
        {
            if(accountPatientId != 0 && accountDoctorID != 0)
            {
                var respone = await _serNoti.GetDateTimeHaveNotification(accountPatientId, accountDoctorID);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        /*
        [HttpPost("SendNotificationCustom")]
        public async Task<IActionResult> SendNotificationCustom(int deviceType,int notificationType, int senderAccountId, int recipientAccountId, string customBody)
        {
            if (deviceType != 0 && notificationType != 0 && senderAccountId != 0 && recipientAccountId != 0 && !string.IsNullOrEmpty(customBody))
            {

            }
            return BadRequest();
        }
        */
    }
}
