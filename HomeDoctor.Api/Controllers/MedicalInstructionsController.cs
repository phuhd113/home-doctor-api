using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HomeDoctor.Business.ViewModel.RequestModel.MIPresciption;

namespace HomeDoctor.Api.Controllers
{
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
        

        public MedicalInstructionsController(IMedicalInstructionService serMI, IImageService serImage, IPatientService serPatient, IDoctorService serDoctor, IAccountService serAccount, IFirebaseFCMService serFireBaseFCM, INotificationService serNoti)
        {
            _serMI = serMI;
            _serImage = serImage;
            _serPatient = serPatient;
            _serDoctor = serDoctor;
            _serAccount = serAccount;
            _serFireBaseFCM = serFireBaseFCM;
            _serNoti = serNoti;
        }



        /// <summary>
        /// Create medical instruction old with image
        /// </summary>
        [HttpPost("InsertMedicalInstructionOld")]
        public async Task<IActionResult> InsertMedicalInstructionOld([FromForm] MedicalInstructionCreate medicalInstruction,IFormFile image)
        {
            if(medicalInstruction.PatientId != 0)
            {
                var patient = await _serPatient.GetPatientInformation(medicalInstruction.PatientId);
                if(patient != null)
                {
                    // generate path of Image
                    var pathImage = Path.Combine(patient.PhoneNumber, "HR" + medicalInstruction.HealthRecordId, "MIT" + medicalInstruction.MedicalInstructionTypeId,image.FileName);
                    // insert MI to DB
                    var check = await _serMI.CreateMedicalInstructionWithImage(medicalInstruction, pathImage);
                    if (check)
                    {
                        // save image to local 
                        check = await _serImage.Upload(patient.PhoneNumber, medicalInstruction.HealthRecordId, medicalInstruction.MedicalInstructionTypeId, image);
                        if (check)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest("Insert Failed");
                        }
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
            if(medicalInstructionId != 0)
            {
                var tmp = await _serMI.GetMedicalInstructionById(medicalInstructionId);
                if(tmp != null)
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
        public async Task<IActionResult> GetMedicalInstructionToCreateContract(int patientId,ICollection<string> diseaseIds, int medicalInstructionType)
        {
            if(patientId != 0)
            {
                var respone = await _serMI.GetMIToCreateContract(patientId, diseaseIds,medicalInstructionType);
                if(respone != null)
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
        public async Task<IActionResult> GetMedicalInstructionsByHealthRecordId(int healthRecordId)
        {
            if(healthRecordId != 0)
            {
                var hrs = await _serMI.GetMedicalInstructionsByHRId(healthRecordId);
                if(hrs != null)
                {
                    return Ok(hrs);
                }
            }
            return NotFound();
        }

        [HttpGet("GetMedicalInstructionToShare")]
        public async Task<IActionResult> GetMedicalInstructionToShare(int patientId, int contractId)
        {
            if(patientId != 0 && contractId != 0)
            {
                var respone = await _serMI.GetMedicalInstructionToShare(patientId, contractId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
    }
}
