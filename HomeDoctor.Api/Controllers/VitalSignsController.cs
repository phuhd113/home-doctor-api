using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.Service;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VitalSignsController : ControllerBase
    {
        private readonly MedicalInstructionService _serMI;

        public VitalSignsController(MedicalInstructionService serMI)
        {
            _serMI = serMI;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVitalSignSchedule(MIVitalSignSchedule request)
        {
            if(request != null)
            {
                var result = await _serMI.CreateVitalSignSchedule(request);
                if(result != 0)
                {
                    return Ok(result);
                }
            }
            return BadRequest();
        }
    }
}
