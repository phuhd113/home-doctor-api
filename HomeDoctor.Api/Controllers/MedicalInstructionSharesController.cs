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
    public class MedicalInstructionSharesController : ControllerBase
    {
        private IMedicalInstructionShareService _serMIS;

        public MedicalInstructionSharesController(IMedicalInstructionShareService serMIS)
        {
            _serMIS = serMIS;
        }
        /// <summary>
        /// Get Medical InstructionShared by contractId
        /// </summary>   
        [HttpGet]
        public async Task<IActionResult> GetMedicalInstructionShare(int contractId)
        
        {
            if(contractId != 0)
            {
                var miShares = await _serMIS.GetMedicalInstructionShare(contractId);
                if(miShares != null)
                {
                    return Ok(miShares);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// Patient share medicalInstructions with Doctor from contractID
        /// </summary>   
        [HttpPost]
        public async Task<IActionResult> ShareMedicalInstruction(int contractId, ICollection<int> medicalInstructionIds)
        {
            if(contractId != 0 && medicalInstructionIds.Count != 0)
            {
                var check = await _serMIS.ShareMedicalInstructionById(contractId, medicalInstructionIds);
                if (check)
                {
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }
    }
}
