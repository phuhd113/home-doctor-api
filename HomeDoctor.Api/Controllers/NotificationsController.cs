using System;
using System.Collections.Generic;
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

        public NotificationsController(INotificationService serNoti, IFirebaseFCMService serFB)
        {
            _serNoti = serNoti;
            _serFB = serFB;
        }


        /// <summary>
        /// Get list notification by accountId. Sort by DateCreate
        /// </summary>      
        [HttpGet]
        public async Task<IActionResult> GetNotificationByAccountId(int accountId)
        {
            if(accountId != 0)
            {
                var noti = await _serNoti.GetNotificationByAccountId(accountId,false);
                if(noti != null)
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
            if(notiId != 0)
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
            if(accountId != 0)
            {
                var history = await _serNoti.GetHistoryByAccountId(accountId);
                if(history != null)
                {
                    return Ok(history);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> SendNotification(int deviceType,int notificationType, int senderAccountId, int recipientAccountId)
        {
            if(notificationType != 0 && senderAccountId != 0 && recipientAccountId != 0)
            {
                // push notification
                await _serFB.PushNotification(deviceType,senderAccountId ,recipientAccountId, notificationType, null, null);
                var noti = new NotificationRequest()
                {
                    AccountSendId = senderAccountId,
                    AccountId = recipientAccountId,
                    NotificationTypeId = notificationType
                };
                var insertNoti = await _serNoti.InsertNotification(noti);
                if (insertNoti)
                {
                    return Ok();
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
    }
}
