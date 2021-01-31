using System;
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
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractSer;
        private readonly IPatientService _patientSer;
        private readonly IDoctorService _doctorSer;
        private readonly ILicenseService _licenseSer;
        private readonly IDiseaseService _diseaseSer;

        public ContractsController(IContractService contractSer, IPatientService patientSer, IDoctorService doctorSer, ILicenseService licenseSer, IDiseaseService diseaseSer)
        {
            _contractSer = contractSer;
            _patientSer = patientSer;
            _doctorSer = doctorSer;
            _licenseSer = licenseSer;
            _diseaseSer = diseaseSer;
        }




        /// <summary>
        /// Patient request contract with status "Pending"
        /// </summary>
        [HttpPost()]
        public async Task<IActionResult> CreateContractByPatient(ContractCreation contract)
        {
            if (contract != null)
            {
                if (!_contractSer.CheckContractToCreateNew(contract.DoctorId, contract.PatientId).Result)
                {

                    var patient = _patientSer.GetPatientInformation(contract.PatientId);
                    var doctor = _doctorSer.GetDoctorInformation(null, contract.DoctorId);
                    var license = _licenseSer.GetLicenseById(contract.LicenseId);                   
                    Task.WaitAll(patient, doctor,license);
                    ICollection<Disease> disenses = null;
                    if(contract.DiseaseIds != null)
                    {
                        disenses = new List<Disease>();
                        foreach(var n in contract.DiseaseIds)
                        {
                            var disease = await _diseaseSer.GetDiseaseById(n);
                            disenses.Add(disease);
                        }                      
                    }
                    if (patient != null && doctor != null)
                    {
                        if (_contractSer.CreateContractByPatient(contract, patient.Result, doctor.Result, license.Result, disenses).Result)
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
        /// Get contract by status
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetContractByStatus(int? doctorId, int? patientId, string? status)
        {
            if (doctorId != null || patientId != null)
            {
                var contracts = _contractSer.GetContractsByStatus(doctorId, patientId, status).Result;
                if (contracts != null)
                {
                    return Ok(contracts);
                }
            }
            else
            {
                return BadRequest();
            }
            return NotFound();
        }
        /// <summary>
        /// Doctor approve  1 contract with can update status or time.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> ApproveConractByDoctor(int contractId, string status, DateTime? dateStart, int? daysOfTracking)
        {
            if (contractId != 0 && !string.IsNullOrEmpty(status))
            {
                var check = _contractSer.UpdateStatuByDoctor(contractId, dateStart, daysOfTracking, status).Result;
                if (check)
                {
                    return NoContent();
                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Get contract by id
        /// </summary>
        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetContractById(int contractId)
        {
            if(contractId != 0)
            {
                var contract = _contractSer.GetContractByContractId(contractId);
                if(contract.Result != null)
                {
                    return Ok(contract.Result);
                }
            }
            return NotFound();
        }
    }
}
