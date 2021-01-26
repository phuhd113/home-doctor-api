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
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractSer;
        private readonly IPatientService _patientSer;
        private readonly IDoctorService _doctorSer;

        public ContractController(IContractService contractSer, IPatientService patientSer, IDoctorService doctorSer)
        {
            _contractSer = contractSer;
            _patientSer = patientSer;
            _doctorSer = doctorSer;
        }



        /// <summary>
        /// Patient request contract with status "Pending"
        /// </summary>
        [HttpPost()]
        public async Task<IActionResult> CreateContractByPatient(ContractCreation contract)
        {
            if(contract != null)
            {
                if(!_contractSer.CheckContractToCreateNew(contract.DoctorId, contract.PatientId).Result)
                {                 
                    
                    var patient = _patientSer.GetPatientInformation(contract.PatientId);
                    var doctor = _doctorSer.GetDoctorInformation(null, contract.DoctorId);
                    Task.WaitAll(patient, doctor);
                    if (patient != null && doctor != null)
                    {
                        if (_contractSer.CreateContractByPatient(contract, patient.Result, doctor.Result).Result)
                        {
                            return StatusCode(201);
                        }
                    }
                }
                else
                {
                    return BadRequest("A contract exists between a doctor and a patient");
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
