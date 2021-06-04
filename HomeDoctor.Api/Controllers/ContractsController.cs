using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _serContract;
        private readonly IPatientService _serPatient;
        private readonly IDoctorService _serDoctor;
        private readonly IFirebaseFCMService _serFirebase;
        private readonly INotificationService _serNotification;

        public ContractsController(IContractService serContract, IPatientService serPatient, IDoctorService serDoctor, IFirebaseFCMService serFirebase, INotificationService serNotification)
        {
            _serContract = serContract;
            _serPatient = serPatient;
            _serDoctor = serDoctor;
            _serFirebase = serFirebase;
            _serNotification = serNotification;
        }




        /// <summary>
        /// Patient request contract with status "Pending"
        /// </summary>
        [HttpPost()]
        public async Task<IActionResult> CreateContractByPatient(ContractCreation contract)
        {
            if (contract != null)
            {
                /*
                // Check exist Contract have status "PENDING" or "ACTIVE" or "APPROVED"between doctor and patient 
                var checkExistContractD = await _serContract.CheckContractToCreateNew(contract.DoctorId, contract.PatientId,null);
                */
                    var patient = await _serPatient.GetPatientInformation(contract.PatientId);
                    var doctor = await _serDoctor.GetDoctorInformation(contract.DoctorId);                
                    if (patient != null && doctor != null)
                    {
                        var contractId = await _serContract.CreateContractByPatient(contract, patient, doctor);
                        if (contractId != 0)
                        {                            
                            // Save notification to DB                         
                            var noti = new NotificationRequest()
                            {
                                ContractId = contractId,
                                AccountId = doctor.AccountId,
                                AccountSendId = patient.AccountId,
                                NotificationTypeId = 1,                               
                            };
                            if(await _serNotification.InsertNotification(noti))
                            {
                                // Firebase Notification for Doctor
                                await _serFirebase.PushNotification(2, patient.AccountId, doctor.AccountId, 1, contractId, null, null,null);
                            }                           
                            return StatusCode(201, "Create Contract success.");
                        }
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
            //Request.Form.Fil
            if (doctorId != null || patientId != null)
            {
                var contracts = await _serContract.GetContractsByStatus(doctorId, patientId, status);
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
        /// Update Status of Contract .Doctor update status "APPROVED" or "CANCELD".Patient update status "ACTIVE" or "CANCELP".
        /// </summary>
        [HttpPut("{contractId}")]
        public async Task<IActionResult> UpdateContract(int contractId,ContractUpdateRequest request)
        {     
            // When doctor or patient CANCEL contract
            if(contractId != 0 && (request.Status.ToUpper().Equals("CANCELD") || request.Status.ToUpper().Equals("CANCELP")))
            {
                var check = await _serContract.UpdateStatusContract(contractId, null, null, request.Status.ToUpper(),null);
                if (check)
                {
                    if (request.Status.ToUpper().Equals("CANCELD"))
                    {
                        // Notification from patient
                        var patient = await _serPatient.GetPatientInformation(request.PatientId);
                        var doctor = await _serDoctor.GetDoctorInformation(request.DoctorId);
                        if (patient != null)
                        {
                            // Save notification
                            var notiRequest = new NotificationRequest()
                            {
                                AccountSendId = doctor.AccountId,
                                AccountId = patient.AccountId,
                                ContractId = contractId,
                                NotificationTypeId = 5
                            };
                            var saveNoti = await _serNotification.InsertNotification(notiRequest);
                            await _serFirebase.PushNotification(1,doctor.AccountId ,patient.AccountId, 5, contractId, null,null,null);
                        }
                    }
                    if (request.Status.ToUpper().Equals("CANCELP"))
                    {
                        // Notification for Doctor
                        var doctor = await _serDoctor.GetDoctorInformation(request.DoctorId);
                        var patient = await _serPatient.GetPatientInformation(request.PatientId);
                        if (doctor != null)
                        {
                            // Save notification
                            var notiRequest = new NotificationRequest()
                            {
                                AccountSendId = patient.AccountId,
                                AccountId = doctor.AccountId,
                                ContractId = contractId,
                                NotificationTypeId = 10
                            };
                            var saveNoti = await _serNotification.InsertNotification(notiRequest);
                            // Firebase Notification for patient
                            await _serFirebase.PushNotification(2,patient.AccountId ,doctor.AccountId, 10, contractId, null,null,null);
                        }
                    }
                    return StatusCode(204, "Cancel contract success");
                }
            }
            else
            {
                if (request.PatientId != 0 && request.DoctorId != 0 && contractId != 0 && !string.IsNullOrEmpty(request.Status))
                {
                    /*
                    // Get all contract of doctor
                    var contracts = await _serContract.GetContractsByStatus(request.DoctorId, null, null);
                    // Check contract has status "ACTIVE" < 5           
                    var contractActive = contracts.Where(x => x.Status.Equals("ACTIVE")).ToList();
                    if (contractActive.Count < 5)
                    {                       
                    */
                        if (request.Status.ToUpper().Equals("APPROVED"))
                        {
                            // update status when active or approved
                            var check = await _serContract.UpdateStatusContract(contractId, request.DateStart, request.DaysOfTracking, request.Status, request.MedicalInstructionChooses);
                            if (check)
                            {
                                // notificate for patient
                                var patient = await _serPatient.GetPatientInformation(request.PatientId);
                                var doctor = await _serDoctor.GetDoctorInformation(request.DoctorId);
                                if (patient != null)
                                {
                                    // Save notification
                                    var notiRequest = new NotificationRequest()
                                    {
                                        AccountSendId = doctor.AccountId,
                                        AccountId = patient.AccountId,
                                        ContractId = contractId,
                                        NotificationTypeId = 4
                                    };
                                    var saveNoti = await _serNotification.InsertNotification(notiRequest);
                                    // Firebase Notification for patient
                                    await _serFirebase.PushNotification(1,doctor.AccountId ,patient.AccountId, 4, contractId, null,null,null);
                                }
                                return StatusCode(204, "Doctor aprroved Contract to success");
                            }
                        }
                        // Patient sign Contract
                        if (request.Status.ToUpper().Equals("SIGNED"))
                        {
                            // update status when active or approved
                            var check = await _serContract.UpdateStatusContract(contractId, null, null,request.Status.ToUpper(), null);
                            if (check)
                            {
                                var doctor = await _serDoctor.GetDoctorInformation(request.DoctorId);
                                var patient = await _serPatient.GetPatientInformation(request.PatientId);
                                if (doctor != null)
                                {
                                    // Save notification of patient for doctor
                                    var notiRequest = new NotificationRequest()
                                    {
                                        AccountSendId = patient.AccountId,
                                        AccountId = doctor.AccountId,
                                        ContractId = contractId,
                                        NotificationTypeId = 9
                                    };                                    
                                    await _serNotification.InsertNotification(notiRequest);   
                                    // Firebase Notification for doctor
                                    await _serFirebase.PushNotification(2,patient.AccountId ,doctor.AccountId, 9, contractId, null,null,null);                                  
                                }
                                return StatusCode(204, "SIGNED contract to success");
                            }                           
                        }
                        if (request.Status.ToUpper().Equals("ACTIVE"))
                        {
                            // update status when active or approved
                            var check = await _serContract.UpdateStatusContract(contractId, null, null, request.Status, null);
                            if (check)
                            {
                                var doctor = await _serDoctor.GetDoctorInformation(request.DoctorId);
                                var patient = await _serPatient.GetPatientInformation(request.PatientId);
                                if (doctor != null)
                                {
                                    //Save notification of system for doctor 
                                    var notiSystem = new NotificationRequest()
                                    {
                                        AccountId = doctor.AccountId,
                                        AccountSendId = patient.AccountId,
                                        ContractId = contractId,
                                        NotificationTypeId = 15,
                                        OnSystem = true
                                    };
                                    await _serNotification.InsertNotification(notiSystem);                                   
                                    // Noti firstTimeAction
                                    await _serFirebase.PushNotification(2, patient.AccountId, doctor.AccountId, 15, contractId, null, null,null);
                                }
                                return StatusCode(204, "active contract to success");
                            }
                        }
                        /*
                    }
                    else
                    {
                        return StatusCode(405, "Contract have status 'ACTIVE' is FULL( >= 5)");
                    }
                */
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
                var contract = await _serContract.GetContractByContractId(contractId);
                if(contract != null)
                {
                    return Ok(contract);
                }
            }
            return NotFound();
        }
        [HttpGet("CheckContractToCreate")]
        public async Task<IActionResult> CheckContractToCreate(int doctorId, int patientId,DateTime? dateStart)
        {
            if(doctorId != 0 && patientId != 0)
            {
                var respone = await _serContract.CheckContractToCreateNew(doctorId, patientId,dateStart);
                if (respone != null)
                {
                    return Ok(respone);
                }
            }
            return NoContent();
        }
        /// <summary>
        /// Demo Locked Contract
        /// </summary>
        [HttpPut("UpdateContractToDemo")]
        public async Task<IActionResult> UpdateContractToDemo(int contractId,string? status,DateTime? timeStarted)
        {
            if(contractId != 0)
            {
                var respone = await _serContract.UpdateContractToDemo(contractId, status, timeStarted);
                if (respone)
                {
                    return StatusCode(204);
                }               
            }
            return BadRequest();
        }

        [HttpGet("GetAllContractByStatus")]
        public async Task<IActionResult> GetAllContractByStatus(string? status)
        {
            var respone = await _serContract.GetAllContractsByAdmin(status);
            if(respone != null)
            {
                return Ok(respone);
            }
            return NotFound();
        }
    }
}
