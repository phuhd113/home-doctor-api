using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HomeDoctor.Business.ViewModel.RequestModel.MIPresciption;

namespace HomeDoctor.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IMedicalInstructionService _serMI;
        private readonly IAccountService _serAccount;
        private readonly INotificationService _serNoti;
        private readonly IFirebaseFCMService _serFireBaseFCM;
        private readonly IActionService _serAction;

        public PrescriptionController(IMedicalInstructionService serMI, IAccountService serAccount, INotificationService serNoti, IFirebaseFCMService serFireBaseFCM, IActionService serAction)
        {
            _serMI = serMI;
            _serAccount = serAccount;
            _serNoti = serNoti;
            _serFireBaseFCM = serFireBaseFCM;
            _serAction = serAction;
        }


        /// <summary>
        /// Create Precription with MedicalInstructiontype = 1 (Medication Schedule).Doctor in the system.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePrescription(MIPresciption request)
        {
            if (request.HealthRecordId != 0)
            {
                var miId = await _serMI.CreatePrescription(request);
                // Notification
                if (miId != 0)
                {
                    var accIds = await _serAccount.GetAccountIdsByHRId(request.HealthRecordId);
                    if (accIds.Count != 0)
                    {                       
                        //Save notification
                        var noti = new NotificationRequest()
                        {
                            AccountSendId = request.DoctorAccountId,
                            AccountId = accIds.Keys.FirstOrDefault(),
                            MedicalInstructionId = miId,
                            NotificationTypeId = 6
                        };
                        await _serNoti.InsertNotification(noti);
                        //var test = await _serMI.
                        await _serFireBaseFCM.PushNotification(1, request.DoctorAccountId, noti.AccountId, 6, null, miId,null,null);
                    }
                    return StatusCode(201,miId);
                }
            }
            return BadRequest();

        }
        /// <summary>
        /// Create Precription with MedicalInstructiontype = 1 (Medication Schedule).Doctor in the system.
        /// </summary>
        [HttpPut()]
        public async Task<IActionResult> UpdatePrecription(int medicalInstructionId, string? status, string reasonCancel, ICollection<MedicationSchedule>? medicationSchedules)
        {
            if (medicalInstructionId != 0)
            {
                var miId = await _serMI.UpdatePrecription(medicalInstructionId, status, reasonCancel, medicationSchedules);
                if (miId != 0)
                {
                    return Ok(miId);
                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Patient Get Prescription by PatientId,Doctor add HealthRecordId.MedicalInsructionType = 1
        /// </summary>      
        [HttpGet("GetPrescriptionByPatientId")]
        public async Task<IActionResult> GetPrescriptionByPatientId([Required] int patientId, int? healthRecordId)
        {
            if (patientId != 0)
            {
                var tmp = await _serMI.GetPrescriptionByPatientId(patientId, healthRecordId);
                if (tmp != null)
                {
                    return Ok(tmp);
                }
            }
            return NotFound();
        }
    }   
}
