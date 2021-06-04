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
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _serApp;
        private readonly IFirebaseFCMService _serFireBase;
        private readonly INotificationService _serNoti;
        private readonly IContractService _serContract;
        private readonly IActionService _serAction;

        public AppointmentsController(IAppointmentService serApp, IFirebaseFCMService serFireBase, INotificationService serNoti, IContractService serContract, IActionService serAction)
        {
            _serApp = serApp;
            _serFireBase = serFireBase;
            _serNoti = serNoti;
            _serContract = serContract;
            _serAction = serAction;
        }
        // Create Appointment by Doctor
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentCreate request)
        {
            if (request != null)
            {
                var checkToCreare = await _serApp.CheckAppointmentToCreate(request.HealthRecordId);
                if (checkToCreare == null)
                {
                    var check = await _serApp.CreateAppointment(request);
                    if (check != 0)
                    {
                        // Check action firstTime
                        if (await _serAction.CheckActionFirstTime(request.HealthRecordId, false, null))
                        {
                            await _serAction.UpdateActionFirstTime(request.HealthRecordId, true, null);
                        }
                        // Save notification
                        var noti = new NotificationRequest()
                        {
                            AccountSendId = request.AccountDoctorId,
                            AccountId = request.AccountPatientId,
                            NotificationTypeId = 8,
                        };
                        await _serNoti.InsertNotification(noti);
                        // Notification firebase
                        await _serFireBase.PushNotification(1, request.AccountDoctorId, request.AccountPatientId, 8, null, null, check, null);

                        return Ok(check);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest(checkToCreare);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Get AppointMent for the month of patient or doctor.
        /// </summary>
        /// <param name="month">2021/03</param>
        /// <returns></returns>
        [HttpGet("GetAppointmentForMonth")]
        public async Task<IActionResult> GetAppointmentForMonth([Required] int accountId, DateTime? month)
        {
            if (accountId != 0)
            {
                var respone = await _serApp.GetAppointmentForMonth(accountId, month);
                if (respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Update appointment.DateExamination from Doctor when status "PENDING". status "ACTIVE" from Patient
        /// </summary>

        [HttpPut("UpdateAppointment")]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, DateTime? dateExamination, string? status, string? diagnose, int contractId)
        {
            if (appointmentId != 0)
            {
                var check = await _serApp.UpdateAppointment(appointmentId, dateExamination, diagnose, status);
                if (check)
                {
                    var contract = await _serContract.GetContractByContractId(contractId);
                    if (contractId != 0)
                    {
                        var noti = new NotificationRequest()
                        {
                            AccountId = contract.AccountDoctorId,
                            AccountSendId = contract.AccountPatientId,
                            AppointmentId = appointmentId,                                 
                        };
                        if (status.ToUpper().Equals("ACTIVE"))
                        {
                            noti.NotificationTypeId = 13;
                            await _serFireBase.PushNotification(1, contract.AccountDoctorId, contract.AccountPatientId, 13, null, null, appointmentId,null);
                        }
                        if (status.ToUpper().Equals("CANCEL"))
                        {
                            noti.NotificationTypeId = 26;
                            await _serFireBase.PushNotification(1, contract.AccountDoctorId, contract.AccountPatientId, 26, null, null, appointmentId,null);
                        }
                        if (status.ToUpper().Equals("FINISHED"))
                        {
                            noti.NotificationTypeId = 28;
                            await _serFireBase.PushNotification(1, contract.AccountDoctorId, contract.AccountPatientId, 28, null, null, appointmentId, null);
                        }
                        await _serNoti.InsertNotification(noti);
                        
                    }
                    return StatusCode(204, "Update Success");
                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Patient cancel appointment wit reasonCancel.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="status"> Doctor cancel with status "CANCELD" . Patient cancel with status "CANCELP"</param>
        /// <param name="reasonCancel"></param>
        /// <returns></returns>
        [HttpPut("CancelAppointment")]
        public async Task<IActionResult> CancelAppointmentByPatient(int appointmentId, string status, string reasonCancel)
        {
            if (appointmentId != 0)
            {
                var check = await _serApp.CancelAppointmentByPatient(appointmentId, reasonCancel);
                if (check)
                {
                    /*
                    if (status.Equals("CANCELP"))
                    {
                        noti.NotificationTypeId = 14;
                        //Push noti firebase
                        await _serFireBase.PushNotification(2, contract.AccountPatientId, contract.AccountDoctorId, 14, null, null);
                    }
                    */
                    // Save noti

                    return StatusCode(204, "Cancel success");
                }
            }
            return BadRequest();
        }
        [HttpGet("GetAppointmentById")]
        public async Task<IActionResult> GetAppointmentById(int appointmentId)
        {
            if (appointmentId != 0)
            {
                var respone = await _serApp.GetAppointmentById(appointmentId);
                if (respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointmentsBetweenDoctorAndPatient([Required]int healthRecordId,string? status)
        {
            if(healthRecordId != 0)
            {
                var respone = await _serApp.GetAppointmentsBetweenDoctorAndPatient(healthRecordId, status);
                if(respone != null)
                {
                    return Ok(respone);
                }
            }
            return NotFound();
        }
    }
}
