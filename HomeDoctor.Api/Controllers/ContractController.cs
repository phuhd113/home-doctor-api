using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractSer;

        public ContractController(IContractService contractSer)
        {
            _contractSer = contractSer;
        }

        /// <summary>
        /// Patient request contract with status "Pending"
        /// </summary>
        [HttpPost("createContract")]
        public async Task<IActionResult> CreateContractByPatient(ContractCreation contract)
        {
            if(contract != null)
            {
                if (_contractSer.CreateContractByPatient(contract).Result)
                {
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Get contract of Doctor by status
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetContractOfDoctorByStatus(int doctorId, string status)
        {
            if(doctorId != 0 && !string.IsNullOrEmpty(status))
            {
                var contracts = _contractSer.GetContractOfDoctorByStatus(doctorId, status).Result;
                if(contracts != null)
                {
                    return Ok(contracts);
                }
            }
            return NotFound();
        }
    }
}
