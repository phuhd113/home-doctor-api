using System;
using System.Collections.Generic;
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
    public class DiseasesController : ControllerBase
    {
        private readonly IDiseaseService _serDisease;

        public DiseasesController(IDiseaseService serDisease)
        {
            _serDisease = serDisease;
        }


        /// <summary>
        /// Get All Disease .
        /// </summary>
        [HttpGet("GetDiseases")]
        public async Task<IActionResult> GetDiseases()
        {
            var diseases = await _serDisease.GetDiseases();
            if(diseases != null)
            {
                return Ok(diseases);
            }
            return NotFound();
        }
        /// <summary>
        /// Insert Disease .
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertDisease(string diseaseId,string code,int? number,int? start, int? end,string name) {
            if(diseaseId != null && name != null)
            {
                var check = await _serDisease.InsertDisease(diseaseId, code, number, start, end, name);
                if (check)
                {
                    return StatusCode(204, "Insert Success");
                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Get Heart Disease .
        /// </summary>
        [HttpGet("GetHeartDiseases")]
        public async Task<IActionResult> GetHeartDiseases()
        {
            var diseases = await _serDisease.GetHeartDiseases();
            if (diseases != null)
            {
                return Ok(diseases);
            }
            return NotFound();
        }
        [HttpGet("GetDiseasesToCreateContract")]
        public async Task<IActionResult> GetDiseasesToCreateContract(int patientId)
        {
            if(patientId != 0)
            {
                var respone = await _serDisease.GetDiseasesToCreateContract(patientId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        [HttpPost("InsertDiseases")]
        public async Task<IActionResult> InsertDiseases(ICollection<DiseaseCreate> diseases)
        {
            if (diseases.Any())
            {
                var respone = await _serDisease.InsertDiseases(diseases);
                if (respone)
                {
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }

        [HttpGet("SearchDiseases")]
        public async Task<IActionResult> SearchDiseases(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var respone = await _serDisease.SearchDiseases(str);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
    }
}
