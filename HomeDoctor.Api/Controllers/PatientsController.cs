using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _serPatient;

        public PatientsController(IPatientService serPatient)
        {
            _serPatient = serPatient;
        }


        /// <summary>
        /// Get Information of Patient by Id.
        /// </summary>
        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            if (patientId != 0)
            {
                var patient =await _serPatient.GetPatientInformation(patientId);
                if(patient != null)
                {
                    return Ok(patient);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// Get patient's information Tracking by doctor with contract status "ACTIVE" by doctorID
        /// </summary>
        [HttpGet("getPatientTrackingByDoctor")]
        public async Task<IActionResult> GetPatientTrackingByDoctor(int doctorId)
        {
            if(doctorId != 0)
            {
                var patient = await _serPatient.GetPatientTrackingByDoctor(doctorId);
                if(patient.Count != 0)
                {
                    return Ok(patient);
                }
            }
            return NotFound();
        }
    }
}
