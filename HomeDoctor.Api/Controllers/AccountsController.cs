using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _ser;
        private readonly IConfiguration _config;

        public AccountsController(IAccountService ser, IConfiguration config)
        {
            _ser = ser;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
           if(!String.IsNullOrEmpty(request.Username) && !String.IsNullOrEmpty(request.Password))
            {
                var check = await _ser.Login(request.Username, request.Password);
                if(check != null)
                {
                    return Ok(new
                    {
                        Token = GenerateJSONWebToken(check)
                    });
                }
            }
            return NotFound();
        }

        [HttpPost("PatientRegister")]
        public async Task<IActionResult> PatientRegister(PatientRegisterRequest request)
        {
            if(!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
            {
                if(await _ser.RegisterPatient(request))
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        private string GenerateJSONWebToken(LoginRespone respone)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            //accountId to save token when login success
            if(respone.RoleId == 1)
            {
                claims.Add(new Claim("accountId", respone.AccountId.ToString()));
                claims.Add(new Claim("doctorId", respone.Id.ToString()));
            }
            if (respone.RoleId == 2)
            {
                claims.Add(new Claim("accountId", respone.AccountId.ToString()));
                claims.Add(new Claim("patientId", respone.Id.ToString()));
            }
            if (respone.RoleId == 3)
            {
                claims.Add(new Claim("accountId", respone.AccountId.ToString()));
            }

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(200),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
