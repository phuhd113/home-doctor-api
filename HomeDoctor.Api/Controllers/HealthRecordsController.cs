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
    public class HealthRecordsController : ControllerBase
    {
        private readonly IHealthRecordService _serHR;

        public HealthRecordsController(IHealthRecordService serHR)
        {
            _serHR = serHR;
        }

        /// <summary>
        /// Get Healthrecords by PatientId. onSystem =true get HR System. onSystem=null getAll.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("GetHealthRecordByPatientId")]
        public async Task<IActionResult> GetHealthRecordByPatientId(int patientId,bool? onSystem = null)
        {
            if(patientId != 0)
            {
                var healthRecords = await _serHR.GetHealthRecordByPatientId(patientId,onSystem);
                if(healthRecords != null)
                {
                    return Ok(healthRecords);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// Get HealthRecord Detail by Id.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetHealthRecordById(int healthRecordId)
        {
            if (healthRecordId != 0)
            {
                var hr = await _serHR.GetHealthRecordById(healthRecordId);
                if(hr != null)
                {
                    return Ok(hr);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// Patient create a Healthrecord to save medical Instruction Old.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateHealthRecord(HealthRecordCreate healthRecord)
        {
            if(healthRecord != null)
            {
                var hrId = await _serHR.CreateHealthRecord(healthRecord);
                if (hrId != 0)
                {
                    return StatusCode(201,hrId);
                }
            }
            return BadRequest();
        }

        [HttpGet("GetOverviewHealthRecordByHRId")]
        public async Task<IActionResult> GetOverviewHealthRecordByHRId(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var respone = await _serHR.GetHROverviewByHRId(healthRecordId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }      
        [HttpPut("{healthRecordId}")]
        public async Task<IActionResult> UpdateHealthRecord(int healthRecordId, HealthRecordUpdate healthRecord)
        {
            if(healthRecordId != 0)
            {
                var respone = await _serHR.UpdateHealthRecord(healthRecordId, healthRecord.Place, healthRecord.Description, healthRecord.Diseases);
                if (respone)
                {
                    return StatusCode(204);
                }
            }
            return BadRequest();
        }       

        [HttpPut("DeleteHealthRecord")]
        public async Task<IActionResult> DeleteHealthRecord([Required]int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var respone = await _serHR.DeleteHealthRecord(healthRecordId);
                if (respone)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Demo 
        /// </summary>
        [HttpPut("UpdateActionFirstTime")]
        public async Task<IActionResult> UpdateActionFirstTimeToDemo(int healthRecordId, bool actionFirstTime)
        {
            if(healthRecordId != 0)
            {
                var respone = await _serHR.UpdateActionFirstTimeToDemo(healthRecordId, actionFirstTime);
                if (respone)
                {
                    return StatusCode(204);
                }
            }
            return BadRequest();
        }
    }
}
