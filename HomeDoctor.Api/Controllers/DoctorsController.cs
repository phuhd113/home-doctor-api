using System;
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
        /// Get Doctor by Id
        /// </summary>      
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetDoctorInformationById(int doctorId)
        {
            if (doctorId != 0)
            {
                var doctorInf = await _serDoc.GetDoctorInformation(doctorId);
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
        
        [HttpGet("GetDoctorTrackingByPatientId")]
        public async Task<IActionResult> GetDoctorTrackingByPatientId(int patientId)
        {
            if(patientId != 0)
            {
                var respone = await _serDoc.GetDoctorTrackingByPatientId(patientId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        [HttpGet("GetAllDoctor")]
        public async Task<IActionResult> GetAllDoctor()
        {
            var respone = await _serDoc.GetAllDoctor();
            if(respone != null)
            {
                return Ok(respone);
            }
            return NotFound();
        }

        [HttpPost("CreateDoctor")]
        public async Task<IActionResult> CreateDoctor(DoctorCreate doctor)
        {
            if(doctor != null)
            {
                var respone = await _serDoc.CreateDoctor(doctor);
                if(respone != 0)
                {
                    return StatusCode(201, respone);
                }
            }
            return BadRequest();
        }
    }
}
