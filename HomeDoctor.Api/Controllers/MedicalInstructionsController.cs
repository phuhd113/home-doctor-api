
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDoctor.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicalInstructionsController : ControllerBase
    {
        private readonly IMedicalInstructionService _serMI;
        private readonly IImageService _serImage;
        private readonly IPatientService _serPatient;
        private readonly IDoctorService _serDoctor;
        private readonly IAccountService _serAccount;
        private readonly IFirebaseFCMService _serFireBaseFCM;
        private readonly INotificationService _serNoti;
        private readonly IContractService _serContract;

        public MedicalInstructionsController(IMedicalInstructionService serMI, IImageService serImage, IPatientService serPatient, IDoctorService serDoctor, IAccountService serAccount, IFirebaseFCMService serFireBaseFCM, INotificationService serNoti, IContractService serContract)
        {
            _serMI = serMI;
            _serImage = serImage;
            _serPatient = serPatient;
            _serDoctor = serDoctor;
            _serAccount = serAccount;
            _serFireBaseFCM = serFireBaseFCM;
            _serNoti = serNoti;
            _serContract = serContract;
        }






        /// <summary>
        /// Create medical instruction old with image
        /// </summary>
        [HttpPost("InsertMedicalInstructionOld")]
        public async Task<IActionResult> InsertMedicalInstructionOld([FromForm] MedicalInstructionCreate medicalInstruction, ICollection<IFormFile> images, string fromBy)
        {
            if (medicalInstruction.PatientId != 0)
            {
                var patient = await _serPatient.GetPatientInformation(medicalInstruction.PatientId);
                if (patient != null)
                {
                    // generate path of many images
                    var pathImages = images.Select(x => Path.Combine(patient.PhoneNumber, "HR" + medicalInstruction.HealthRecordId, "MIT" + medicalInstruction.MedicalInstructionTypeId, x.FileName)).ToList();
                    // insert MI to DB
                    var check = await _serMI.CreateMedicalInstructionWithImage(medicalInstruction, pathImages, fromBy);
                    if (check)
                    {
                        // save image to local 
                        if (images.Any())
                        {
                            foreach (var n in images)
                            {
                                await _serImage.Upload(patient.PhoneNumber, medicalInstruction.HealthRecordId, medicalInstruction.MedicalInstructionTypeId, n);
                            }
                        }
                        return Ok();
                    }

                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Get Medical Instruction by Id
        /// </summary>      
        [HttpGet("{medicalInstructionId}")]
        public async Task<IActionResult> GetMedicalInstructionById(int medicalInstructionId)
        {
            if (medicalInstructionId != 0)
            {
                var tmp = await _serMI.GetMedicalInstructionById(medicalInstructionId);
                if (tmp != null)
                {
                    return Ok(tmp);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// Get All Medical Instruction of Health Record have the same DieaseId .
        /// </summary>      
        [HttpPost("GetMedicalInstructionToCreateContract")]
        public async Task<IActionResult> GetMedicalInstructionToCreateContract(int patientId, string? diseaseId, int? medicalInstructionTypeId, ICollection<int>? medicalInstructionId)
        {
            if (patientId != 0)
            {
                var respone = await _serMI.GetMIToCreateContract(patientId, diseaseId, medicalInstructionTypeId, medicalInstructionId);
                if (respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }


        /// <summary>
        /// Get MedicalInstrucs of Patient by HealthRecordId. 
        /// </summary>     
        [HttpGet("GetMedicalInstructionsByHRId")]
        public async Task<IActionResult> GetMedicalInstructionsByHealthRecordId([Required] int healthRecordId, int? medicalInstructionTypeId)
        {
            if (healthRecordId != 0)
            {
                var hrs = await _serMI.GetMedicalInstructionsByHRId(healthRecordId, medicalInstructionTypeId);
                if (hrs != null)
                {
                    return Ok(hrs);
                }
            }
            return NotFound();
        }

        [HttpGet("GetMedicalInstructionsToShare")]
        public async Task<IActionResult> GetMedicalInstructionsToShare([Required]int patientId,[Required] int healthRecordId,int? medicalInstructionType)
        {
            if (patientId != 0 && healthRecordId != 0)
            {
                var respone = await _serMI.GetMedicalInstructionsToShare(patientId, healthRecordId,medicalInstructionType);
                if (respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// Patient share medicalInstructions with Doctor from contractID
        /// </summary>     
        [HttpPost("ShareMedicalInstructions")]
        public async Task<IActionResult> ShareMedicalInstructions(int healthRecordId, ICollection<int> medicalInstructionIds,int contractId)
        {
            if (healthRecordId != 0 && medicalInstructionIds.Any())
            {
                var check = await _serMI.ShareMedicalInstructions(healthRecordId, medicalInstructionIds);
                if (check)
                {
                    if(contractId != 0)
                    {
                        var contract = await _serContract.GetContractByContractId(contractId);
                        if(contract != null)
                        {
                            var noti = new NotificationRequest()
                            {
                                AccountId = contract.AccountDoctorId,
                                AccountSendId = contract.AccountPatientId,
                                NotificationTypeId = 3,
                                OnSystem = false,                                 
                            };
                            await _serNoti.InsertNotification(noti);
                            await _serFireBaseFCM.PushNotification(2, noti.AccountSendId.GetValueOrDefault(), noti.AccountId, 3, null, null, null, null);
                        }
                    }
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }
        [HttpPost("DeleteMedicalInstruction")]
        public async Task<IActionResult> DeleteMedicalInstruction([Required] int medicalInstructionId)
        {
            if (medicalInstructionId != 0)
            {
                var respone = await _serMI.DeleteMedicalInstruction(medicalInstructionId);
                if (respone)
                {
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }

        [HttpPost("AddMedicalInstructionFromContract")]
        public async Task<IActionResult> AddMedicalInstructionFromContract([Required] int contractId, ICollection<int> medicalInstructionIds)
        {
            if (contractId != 0 && medicalInstructionIds.Any())
            {
                var respone = await _serMI.AddMedicalInstructionFromContract(contractId, medicalInstructionIds);
                if (respone)
                {
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }
        [HttpPut("UpdateStatusMedicalInstruction")]
        public async Task<IActionResult> UpdateStatusMedicalInstruction([Required]int medicalInstructionId,[Required] string status)
        {
            if(medicalInstructionId != 0 && !string.IsNullOrEmpty(status))
            {
                var respone = await _serMI.UpdateStatusMedicalInstruction(medicalInstructionId, status);
                if (respone)
                {
                    return StatusCode(204);
                }
            }
            return BadRequest();
        }
    }
}
