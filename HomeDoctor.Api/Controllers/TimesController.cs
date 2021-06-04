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
    public class TimesController : ControllerBase
    {
        private readonly ITimeService _serTime;

        public TimesController(ITimeService serTime)
        {
            _serTime = serTime;
        }
        [HttpPost]
        public async Task<IActionResult> ChangeSystemTime(DateTime dateTime,bool onLinux)
        {
            if(dateTime != null)
            {
                await _serTime.UpdateSystemTime(dateTime,onLinux);
            }
            return StatusCode(201);
        }

        /*
        [HttpPost("ResetSystemTime")]
        public async Task<IActionResult> ResetSystemTime()
        {
            await _serTime.ResetSystemTime();
            return StatusCode(201);
        }
        */
        [HttpGet]
        public async Task<IActionResult> GetTimeSystem()
        {
            var currentTime = DateTime.Now;
            if(currentTime != null)
            {
                return Ok(currentTime);
            }
            return BadRequest();
        }
    }
}
