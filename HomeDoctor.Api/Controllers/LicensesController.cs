﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private readonly ILicenseService _serLicense;

        public LicensesController(ILicenseService serLicense)
        {
            _serLicense = serLicense;
        }


        /// <summary>
        /// Get Licenses by status.status = null to getAll
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLicenses(string? status)
        {
            var licenses = await _serLicense.GetLicensesByStatus(status);
            if (licenses != null)
            {
                return Ok(licenses);
            }
            return NotFound();
        }
        [HttpGet("GetLicenseByDays")]
        public async Task<IActionResult> GetLicenseByDays(int days)
        {
            if(days != 0)
            {
                var license = await _serLicense.GetLicenseByDays(days);
                if(license != null)
                {
                    return Ok(license);
                }
            }
            return NotFound();
        }
        [HttpPost("CreateLicense")]
        public async Task<IActionResult> CreateLicense(LicenseCreate license)
        {
           if(license != null)
            {
                var respone = await _serLicense.CreateLicense(license);
                if(respone != 0)
                {
                    return StatusCode(201, respone);
                }
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateLicense(License lisence)
        {
            var tmp = await _serLicense.UpdateLicense(lisence);
            if (tmp)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLicense(int lisenceId)
        {
            var tmp = await _serLicense.DeleteLicense(lisenceId);
            if (tmp)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
