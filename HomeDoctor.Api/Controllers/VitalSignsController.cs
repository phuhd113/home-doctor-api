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

namespace HomeDoctor.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VitalSignsController : ControllerBase
    {
        private readonly IMedicalInstructionService _serMI;
        private readonly IVitalSignService _serVitalSign;
        private readonly IAccountService _serAcc;
        private readonly INotificationService _serNoti;
        private readonly IFirebaseFCMService _serFireBaseFCM;
        private readonly IActionService _serAction;

        public VitalSignsController(IMedicalInstructionService serMI, IVitalSignService serVitalSign, IAccountService serAcc, INotificationService serNoti, IFirebaseFCMService serFireBaseFCM, IActionService serAction)
        {
            _serMI = serMI;
            _serVitalSign = serVitalSign;
            _serAcc = serAcc;
            _serNoti = serNoti;
            _serFireBaseFCM = serFireBaseFCM;
            _serAction = serAction;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVitalSignSchedule(MIVitalSignSchedule request)
        {
            if (request != null)
            {
                var result = await _serMI.CreateVitalSignSchedule(request);
                if (result != 0)
                {
                    // Check ActionFirstTime 
                    if (await _serAction.CheckActionFirstTime(request.HealthRecordId, null, false))
                    {
                        await _serAction.UpdateActionFirstTime(request.HealthRecordId, null, true);
                    }
                    var accIds = await _serAcc.GetAccountIdsByHRId(request.HealthRecordId);
                    if(accIds.Count != 0)
                    {
                        //Save notification
                        var noti = new NotificationRequest()
                        {
                            AccountSendId = request.DoctorAccountId,
                            AccountId = accIds.Keys.FirstOrDefault(),
                            MedicalInstructionId = result,
                            NotificationTypeId = 7
                        };
                        // Save noti
                        await _serNoti.InsertNotification(noti);
                        // push noti firebase
                        await _serFireBaseFCM.PushNotification(1, request.DoctorAccountId, noti.AccountId, 7, null, result, null,null);
                    }
                    return StatusCode(201, result);
                }
            }
            return BadRequest();
        }
        /*
        [HttpPost]
        public async Task<IActionResult> InsertHeartBeat(int vitalSignId, string heartRate)
        {
            if(vitalSignId != 0 && !string.IsNullOrEmpty(heartRate))
            {

            }
            return BadRequest();
        }
        */
        [HttpGet]
        public async Task<IActionResult> GetVitalSignSchedule(int patientId,string? status)
        {
            if(patientId != 0)
            {
                var schedule = await _serMI.GetVitalSignScheduleByPatientId(patientId, status);
                if (schedule != null)
                {
                    return Ok(schedule);
                }
            }
            return NotFound();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateVitalSignValueByDate(VitalSignValueRequest request)
        {
            //var date = DateTime.Now;;           
                if(request.PatientId != 0 && request.VitalSignTypeId != 0)
                {
                    var update = await _serVitalSign.UpdateVitalSignValue(request.PatientId, request.VitalSignTypeId, request.TimeValue, request.NumberValue);
                    if (update)
                    {
                        return StatusCode(204);
                    }
                }
            return BadRequest();
        }

        [HttpGet("GetVitalSignValueByPatientId")]
        public async Task<IActionResult> GetVitalSignValueByPatientId([Required] int patientId,int? healthRecordId,[Required]DateTime dateTime)
        {
            if(healthRecordId != 0)
            {
                var respone = await _serVitalSign.GetVitalSignValueByPatientId(patientId,healthRecordId,dateTime);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        [HttpGet("GetVitalSignValueByMIId")]
        public async Task<IActionResult> GetVitalSignValueByMIId([Required]int medicalInstructionId,[Required]int patientId)
        {
            if(medicalInstructionId != 0 && patientId != 0)
            {
                var respone = await _serVitalSign.GetVitalSignValueByMIId(medicalInstructionId, patientId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }

        [HttpGet("GetVitalSignScheduleByHRId")]
        public async Task<IActionResult> GetVitalSignScheduleByHRId([Required] int healthRecordId)
        {
            if (healthRecordId != 0)
            {
                var respone = await _serVitalSign.GetVitalSignScheduleByHRId(healthRecordId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        [HttpGet("GetDateTimeHaveVitalSignValue")]
        public async Task<IActionResult> GetDateTimeHaveVitalSignValue([Required]int patientId,int? healthRecordId)
        {
            if(patientId != 0)
            {
                var respone = await _serVitalSign.GetDateTimeHaveVitalSignValue(patientId, healthRecordId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }

        [HttpPost("ShareVitalSignValue")]
        public async Task<IActionResult> ShareVitalSignValue(int healthRecordId, [FromQuery]DateTime timeShare,int minuteShare)
        {
            if(healthRecordId != 0 && timeShare != null && minuteShare != 0)
            {
                var respone = await _serVitalSign.ShareVitalSignValue(healthRecordId, timeShare, minuteShare);
                if (respone != 0)
                {
                    //Dictinary with key is accPatient, value accDoctor
                    var accIds = await _serAcc.GetAccountIdsByHRId(healthRecordId);
                    if(accIds.Count != 0)
                    {
                        var noti = new NotificationRequest()
                        {
                            AccountSendId = accIds.Keys.FirstOrDefault(),
                            AccountId = accIds.Values.FirstOrDefault(),
                            VitalSignValueShareId = respone,
                            NotificationTypeId = 29,
                            OnSystem = false,                          
                        };
                        // Save noti
                        await _serNoti.InsertNotification(noti);
                        // push noti firebase
                        await _serFireBaseFCM.PushNotification(2, noti.AccountSendId.GetValueOrDefault(), noti.AccountId,29,null, respone,null,null);
                    }
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }
        [HttpGet("GetVitalSignsType")]
        public async Task<IActionResult> GetVitalSignsType()
        {
            var respone = await _serVitalSign.GetVitalSignsType();
            if (respone.Any())
            {
                return Ok(respone);
            }
            return NotFound();
        }
        /// <summary>
        /// Get vital Sign Value When Patient share and notifi for doctor
        /// </summary>
        [HttpGet("GetVitalSignShareById")]
        public async Task<IActionResult> GetVitalSignShareById(int vitalSignShareId)
        {
            if(vitalSignShareId != 0)
            {
                var respone = await _serVitalSign.GetVitalSignValueShareById(vitalSignShareId);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Get vital Sign Value Share By Date , date == null to Getall
        /// </summary>
        [HttpGet("GeVitalSignShareByDate")]
        public async Task<IActionResult> GeVitalSignShareByDate([Required]int healthRecordId,DateTime? dateTime)
        {
            if (healthRecordId != 0)
            {
                var respone = await _serVitalSign.GetVitalSignValueShareByDate(healthRecordId, dateTime);
                if (respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
    }
}
