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
    public class DiseasesController : ControllerBase
    {
        private readonly IDiseaseService _diseaseSer;

        public DiseasesController(IDiseaseService diseaseSer)
        {
            _diseaseSer = diseaseSer;
        }
        /// <summary>
        /// Get Disease by status.status = null to getAll
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDiseases(string? status)
        {
            var diseases = _diseaseSer.GetDiseases(status);
            if(diseases.Result != null)
            {
                return Ok(diseases.Result);
            }
            return NotFound();
        }
    }
}
