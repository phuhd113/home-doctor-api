using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonalHealthReocrdsController : ControllerBase
    {
        private readonly IPersonalHealthRecordService _serPersonal;

        public PersonalHealthReocrdsController(IPersonalHealthRecordService serPersonal)
        {
            _serPersonal = serPersonal;
        }
        /// <summary>
        /// Update when connect smart watch or disconect
        /// </summary>
        
        [HttpPut("UpdateSmartWatchConnected")]
        public async Task<IActionResult> UpdateSmartWatchConnect([Required] int patientId, [Required] bool isConnected)
        {
            if(patientId != 0)
            {
                var check = await _serPersonal.UpdateSmartWatchConnect(patientId, isConnected);
                if (check)
                {
                    return Ok(204);
                }
            }
            return BadRequest();
        }
    }
}
