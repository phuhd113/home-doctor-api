using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FireBasesController : ControllerBase
    {
        private readonly IFirebaseFCMService _serFB;

        public FireBasesController(IFirebaseFCMService serFB)
        {
            _serFB = serFB;
        }
        /// <summary>
        /// Save token of patient or doctor when Login success.
        /// </summary>    
        [HttpPost]
        public async Task<IActionResult> SaveTokenDevice([FromForm]int accountId,[FromForm]string? token)
        {
            if (accountId != 0)
            {
                var saveToken = await _serFB.SaveToken(accountId, token);
                if (saveToken)
                {
                    return StatusCode(201, "Save Token Device success");
                }
            }
            return BadRequest();
        }
        //[HttpPost("TestNotification")]
        //public async Task<IActionResult> TestNotification(int accountId)
        //{
        //    if(accountId != 0)
        //    {
        //        //var respone = await _serFB.PushNotificationMobile(accountId,1);
        //        //Console.WriteLine(respone);
        //    }
        //    return BadRequest();
        //}
    }
}
