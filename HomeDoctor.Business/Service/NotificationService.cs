using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryBase<Notification> _repo;
        private readonly IFirebaseFCMService _serFB;
        private readonly IUnitOfWork _uow;

        public NotificationService(IUnitOfWork uow, IFirebaseFCMService serFB)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Notification>();
            _serFB = serFB;
        }

        public async Task<ICollection<HistoryRespone>> GetHistoryByAccountId(int accountId)
        {
            if(accountId != 0)
            {
                var listHistory = await _repo.GetDbSet().Where(x => x.AccountSendId == accountId).Include(x => x.Account).Include(x => x.NotificationType).ToListAsync();
                if(listHistory.Any())
                {
                    var timeNow = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7);
                    var respone = listHistory.GroupBy(x => x.DateCreate.ToString("dddd , dd/MM/yyyy")).Select(
                        x => new HistoryRespone()
                        {
                            DateCreate = x.Key,
                            Histories = x.Select(y => new HistoryRespone.History
                            {
                                Title = y.NotificationType.Title,
                                Body = this.GenarateHistoryBody(y.NotificationTypeId, y.Account.FullName),
                                HistoryType = y.NotificationTypeId,
                                ContractId = y.ContractId,
                                MedicalInstructionId = y.MedicalInstructionId,
                                TimeAgo = (timeNow.Subtract(y.DateCreate).TotalMinutes).ToString()
                            }).OrderBy(x => x.TimeAgo).ToList()
                        }).OrderBy(x => x.DateCreate).ToList();
                    
                    return respone;
                }
            }
            return null;
        }

        public async Task<ICollection<NotificationRespone>> GetNotificationByAccountId(int accountId, bool onSystem)
        {
            if(accountId != 0)
            {
                // Get all noti
                var noti = await _repo.GetDbSet().Include(x => x.NotificationType).Where(x => x.AccountId == accountId && x.OnSystem == onSystem).ToListAsync();

                if (noti.Any())
                {
                    var timeNow = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7);
                    var respone = noti.OrderByDescending(x => x.DateCreate)
                        .GroupBy(x => x.DateCreate.ToString("dddd, dd/MM/yyyy")).Select(x => new NotificationRespone()
                    {
                        DateCreate = x.Key,
                        Notifications = x.Select(x => new NotificationRespone.Notification()
                        {
                            NotificationId = x.NotificationId,
                            Title = x.NotificationType.Title,
                            Body = _serFB.GenarateBodySend(x.NotificationTypeId, x.AccountSendId.GetValueOrDefault(), x.NotificationType.Body).Result,
                            TimeAgo = (timeNow.Subtract(x.DateCreate).TotalMinutes).ToString(),
                            Status = x.Status,
                            ContractId = x.ContractId,
                            MedicalInstructionId = x.MedicalInstructionId,
                            NotificationType = x.NotificationTypeId
                        }).OrderBy(x => x.TimeAgo).ToList()
                    }).ToList();
                    if (respone.Any())
                    {
                        return respone;
                    }
                }
                    
                    /*
                    Select(x => new NotificationRespone()
                {
                    NotificationId = x.NotificationId,
                    Title = x.NotificationType.Title,
                    Body = _serFB.GenarateBodySend(x.NotificationTypeId,x.AccountSendId,x.NotificationType.Body).Result,
                    Status = x.Status,
                    DateCreate = x.DateCreate,
                    ContractId = x.ContractId,
                    MedicalInstructionId = x.MedicalInstructionId,
                    NotificationType = x.NotificationTypeId
                    
                }).OrderByDescending(x => x.DateCreate).ToListAsync();
                    */               
            }
            return null;
        }
        public async Task<bool> InsertNotification(NotificationRequest notification)
        {
            if(notification != null)
            {
                var noti = new Notification()
                {
                    Status = false,
                    AccountId = notification.AccountId,
                    AccountSendId = notification.AccountSendId.GetValueOrDefault(),
                    ContractId = notification.ContractId,
                    MedicalInstructionId = notification.MedicalInstructionId,
                    NotificationTypeId = notification.NotificationTypeId,       
                    DateCreate = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now : DateTime.Now.AddHours(7),
                    OnSystem = notification.OnSystem
                };
                var check = await _repo.Insert(noti);
                if (check)
                {
                    await _uow.CommitAsync();
                    return check;
                }
            }
            return false;
        }

        public async Task<bool> UpdateStatusNotificationByNotiId(int notiId)
        {
            if(notiId != 0)
            {
                var noti = await _repo.GetById(notiId);
                if(noti != null)
                {
                    noti.Status = true;
                }
                var update = await _repo.Update(noti);
                if (update)
                {
                    await _uow.CommitAsync();
                    return true;
                }
            }
            return false;
        }
        public string GenarateHistoryBody(int historyType, string? fullName)
        {
            if (historyType != 0 && fullName != null)
            {
                string respone = null;
                switch (historyType)
                {
                    case 1:
                        respone = "Bạn đã gửi hợp đồng yêu cầu theo dõi với bác sĩ " + fullName;
                        break;
                    case 3:
                        respone = "Bạn đã chia sẽ hồ sơ bệnh án với bác sĩ " + fullName;
                        break;
                    case 4:
                        respone = "Bác sĩ đã chấp thuận yêu cầu theo dõi với bệnh nhân " + fullName;
                        break;
                    case 5:
                        respone = "Bác sĩ đã từ chối yêu cầu theo dõi với bệnh nhân " + fullName;
                        break;
                    case 6:
                        respone = "Bác sĩ đã tạo một đơn thuốc mới cho bệnh nhân " + fullName;
                        break;
                    case 7:
                        respone = "Bác sĩ đã tạo một lịch đo sinh hiệu mới cho bệnh nhân " + fullName;
                        break;
                    case 8:
                        respone = "Bác sĩ đã tạo một cuộc hẹn thăm khám cho bệnh nhân " + fullName;
                        break;
                    case 9:
                        respone = "Bạn đã ký xác nhận hợp đồng với bác sĩ  " + fullName;
                        break;
                    case 10:
                        respone = "Bạn đã hủy hợp đồng với bác sĩ  " + fullName;
                        break;
                    case 11:
                        respone = "Bạn đã yêu cầu bệnh nhân " + fullName + " kết nối thiết bị đồng hồ thông minh ";
                        break;
                    case 12:
                        respone = "Bạn đã yêu cầu bệnh nhân " + fullName + " chia sẽ thêm các phiếu y lệnh ";
                        break;

                }
                return respone;
            }
            return null;
        }
    }
}
