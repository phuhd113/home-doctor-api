﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _serDoc;
        private readonly IConfiguration _config;

        public DoctorsController(IDoctorService serDoc, IConfiguration config)
        {
            _serDoc = serDoc;
            _config = config;
        }
        /// <summary>
        /// Login of Doctor with username and password.
        /// </summary>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDoctor login)
        {
            var doctor = _serDoc.Login(login.Username, login.Password).Result;
            if (doctor != null)
            {
                return Ok(new { Token = GenerateJSONWebToken(doctor.DoctorId)});
            }
            return NotFound();
        }

        /// <summary>
        /// Get Doctor by username
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetDoctorInformationByUsername(string username)
        {
            if(username != null)
            {
                var doctorInf = _serDoc.GetDoctorInformation(username,0).Result;
                if(doctorInf != null)
                {
                    return Ok(doctorInf);
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Get Doctor by Id
        /// </summary>      
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetDoctorInformationById(int doctorId)
        {
            if (doctorId != 0)
            {
                var doctorInf = _serDoc.GetDoctorInformation(null, doctorId).Result;
                if (doctorInf != null)
                {
                    return Ok(doctorInf);
                }
            }
            return NotFound();
        }
        private string GenerateJSONWebToken(int doctorId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim("doctorId", doctorId.ToString()));
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}