using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private readonly ILicenseService _licenseSer;

        public LicensesController(ILicenseService licenseSer)
        {
            _licenseSer = licenseSer;
        }
        /// <summary>
        /// Get Licenses by status.status = null to getAll
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLicenses(string? status)
        {
            var licenses = _licenseSer.GetLicensesByStatus(status);
            if (licenses.Result != null)
            {
                return Ok(licenses.Result);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddLicense(string name,int days,float price,string description)
        {
            var license = new License()
            {
                Name = name,
                Days = days,
                Price = price,
                Description = description,
                Status = "active"
            };
            var tmp = _licenseSer.AddLicense(license);
            if (tmp.Result)
            {
                return StatusCode(201);
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateLicense(License lisence)
        {
            var tmp = _licenseSer.UpdateLicense(lisence);
            if (tmp.Result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLicense(int lisenceId)
        {
            var tmp = _licenseSer.DeleteLicense(lisenceId);
            if (tmp.Result)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
