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
    public class MedicalInstructrionTypesController : ControllerBase
    {
        private readonly IMedicalInstructionTypeService _service;

        public MedicalInstructrionTypesController(IMedicalInstructionTypeService service)
        {
            _service = service;
        }
        /// <summary>
        /// Get MedicalInstruoctionType by status(active or cancel)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMedicalInstructionTypeByStatus(string? status)
        {
            var MITypes = await _service.GetMedicalInstructionTypeByStatus(status);
            if(MITypes != null)
            {
                return Ok(MITypes);
            }
            return NotFound();
        }
    }
}
