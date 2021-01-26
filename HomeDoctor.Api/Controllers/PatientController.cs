using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientSer;

        public PatientController(IPatientService patientSer)
        {
            _patientSer = patientSer;
        }
        /// <summary>
        /// Get Information of Patient by Id.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            if (patientId != 0)
            {
                var patient = _patientSer.GetPatientInformation(patientId).Result;
                if(patient != null)
                {
                    return Ok(patient);
                }
            }
            return NotFound();
        }
    }
}
