using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Api.Signalr;
using HomeDoctor.Business.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SignalrsController : ControllerBase
    {
        private readonly IHubContext<SignalrHub, IHubClient> _signalrHub;
        private readonly IPatientService _serPatient;

        public SignalrsController(IHubContext<SignalrHub, IHubClient> signalrHub, IPatientService serPatient)
        {
            _signalrHub = signalrHub;
            _serPatient = serPatient;
        }

        [HttpGet("{patientId:int}")]
        [Authorize]
        public async Task<IActionResult> SendContractInformation(int patientId)
        {
            var patient = _serPatient.GetPatientInformation(patientId).Result;
            if(patient != null)
            {
                await _signalrHub.Clients.All.SendNotificationNewContract(patient);
                return Ok(patient);
            }
            return NotFound();
        }

    }
}
