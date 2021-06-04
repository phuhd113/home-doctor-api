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
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _uow;

        public NotificationService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Notification>();
            _repoAccount = _uow.GetRepository<Account>();
        }

        public async Task<ICollection<HistoryRespone>> GetHistoryByAccountId(int accountId)
        {
            if(accountId != 0)
            {
                var listHistory = await _repo.GetDbSet().Where(x => x.AccountSendId == accountId).Include(x => x.Account).Include(x => x.NotificationType).ToListAsync();
                if(listHistory.Any())
                {
                    var timeNow = DateTime.Now;
                    var respone = listHistory.OrderByDescending(x => x.DateCreate).GroupBy(x => x.DateCreate.ToString("dddd , dd/MM/yyyy")).Select(
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
                                TimeAgo = timeNow.Subtract(y.DateCreate).TotalMinutes,
                            }).OrderBy(x => x.TimeAgo).ToList()
                        }).ToList();
                    
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
                var noti = await _repo.GetDbSet().Include(x => x.NotificationType).Where(x => x.AccountId == accountId && (x.Account.Doctor != null ? x.OnSystem == onSystem : true)).ToListAsync();
                if (noti.Any())
                {
                    var timeNow = DateTime.Now;
                    var respone = noti.OrderByDescending(x => x.DateCreate)
                        .GroupBy(x => x.DateCreate.ToString("dddd, dd/MM/yyyy")).Select(x => new NotificationRespone()
                        {
                            DateCreate = x.Key,
                            Notifications = x.Select(x => new NotificationRespone.Notification()
                            {
                                NotificationId = x.NotificationId,
                                Title = x.NotificationType.Title,
                                Body = GenarateBodySend(x.NotificationTypeId, x.AccountSendId.GetValueOrDefault(),x.BodyCustom).Result,
                                TimeAgo = timeNow.Subtract(x.DateCreate).TotalMinutes,
                                Status = x.Status,
                                ContractId = x.ContractId,
                                MedicalInstructionId = x.MedicalInstructionId,
                                NotificationType = x.NotificationTypeId,
                                AppointmentId = x.AppointmentId
                            }).OrderBy(x => x.TimeAgo).ToList()
                        }).ToList();
                    //OrderBy(x => DateTime.Parse(x.DateCreate)).ToList();
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
                    DateCreate = DateTime.Now,
                    OnSystem = notification.OnSystem,
                    BodyCustom = notification.bodyCustom,
                    AppointmentId = notification.AppointmentId,
                    VitalSignShareId = notification.VitalSignValueShareId
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
                        respone = "Bác sĩ đã yêu cầu bệnh nhân " + fullName + " kết nối thiết bị đồng hồ thông minh ";
                        break;
                    case 13:
                        respone = "Bác sĩ đã thay đổi ngày hẹn với bệnh nhân " + fullName + ".";
                        break;
                    case 14:
                        respone = "Bệnh nhân " + fullName + " đã thay đổi ngày hẹn với bác sĩ.";
                        break;
                    case 15:
                        respone = "Hợp đồng theo dõi giữa bác sĩ và bệnh nhân " + fullName + ".";
                        break;
                    case 16:
                        respone = "Hợp đồng theo dõi giữa bác sĩ và bệnh nhân " + fullName + " đã bị khóa bởi hệ thống.";
                        break;
                    case 17:
                        respone = "Bệnh nhân " + fullName + " đã trở lại trạng thía bình thường.";
                        break;
                    case 19:
                        respone = "Hợp đồng giữa bác sĩ và bệnh nhân " + fullName + " đã hoàn thành.";
                        break;
                    case 22:
                        respone = "Hôm nay bác sĩ có cuộc hẹn với bệnh nhân " + fullName + ".";
                        break;
                    case 24:
                        respone = "Bác sĩ đã thêm y lệnh vào hồ sơ của bệnh nhân " + fullName + ".";
                        break;
                    case 26:
                        respone = "Bác sĩ đã hủy cuộc hẹn với bệnh nhân " + fullName + ".";
                        break;
                    case 27:
                        respone = "Bác sĩ đã yêu cầu khẩn cấp với bệnh nhân " + fullName + ".";
                        break;
                    case 28:
                        respone = "Bác sĩ đã xác nhận hoàn thành cuộc hẹn với bệnh nhân "+ fullName +".";
                        break;

                }
                return respone;
            }
            return null;
        }

        // For Firebase
        public async Task<string> GenarateBodySend(int notiType, int senderAccountId, string? bodyCustom)
        {
            if (notiType != 0)
            {
                var body = "";
                var fullNameSender = await _repoAccount.GetDbSet().Where(x => x.AccountId == senderAccountId).Select(x => x.FullName).FirstOrDefaultAsync();
                switch (notiType)
                {
                    case 1:
                        body = "Bác sĩ có một hợp đồng yêu cầu theo dõi mới từ bệnh nhân " + fullNameSender + ".";
                        break;
                    case 2:
                        body = "Bệnh nhân "+fullNameSender+ " trong danh sách theo dõi có trạng thái bất thường.";
                        break;
                    case 3:
                        body = "Bệnh nhân " + fullNameSender + " trong danh sách theo dõi đã chia sẽ thêm y lệnh của họ cho bác sĩ.";
                        break;
                    case 4:
                        body = "Hợp đồng yêu cầu theo dõi của bạn đã được chấp thuận từ bác sĩ " + fullNameSender;
                        break;
                    case 5:
                        body = "Hợp đồng yêu cầu theo dõi của bạn đã bị từ chối từ bác sĩ " + fullNameSender;
                        break;
                    case 6:
                        body = "Bệnh nhân có một lịch uống thuốc mới từ bác sĩ " + fullNameSender;
                        break;
                    case 7:
                        body = "Bệnh nhân có một lịch đo sinh hiệu mới từ bác sĩ " + fullNameSender;
                        break;
                    case 8:
                        body = "Bệnh nhân có một cuộc hẹn thăm khám với bác sĩ " + fullNameSender;
                        break;
                    case 9:
                        body = "Bệnh nhân "+fullNameSender+" đã ký xác nhận hợp đồng .Hợp đồng theo dõi giữa bác sĩ và bệnh nhân sẽ có hiệu lực khi đến ngày bắt đầu theo dõi trên hợp đồng.";
                        break;
                    case 10:
                        body = "Bệnh nhân " + fullNameSender + " đã không chấp thuận hợp đồng với bác sĩ.";
                        break;                       
                    case 11:
                        body = "Bệnh nhân có yêu cầu kết nối thiết bị đồng hồ thông mình từ bác sĩ " + fullNameSender;
                        break;
                    case 12:
                        body = "Bác sĩ " + fullNameSender +" đã yêu cầu bạn chia sẽ thêm phiếu y lệnh ("+ bodyCustom + ")";
                        break;
                    case 13:
                        body = "Bác sĩ " + fullNameSender + " đã thay đổi ngày tái khám. Bệnh nhân vui lòng kiểm tra lại để không nhỡ cuộc hẹn với bác sĩ.";
                        break;
                    case 14:
                        body = "Bệnh nhân " + fullNameSender + " mong muốn thay đổi ngày tái khám. Bác sĩ vui lòng kiểm tra và xác nhận để không nhỡ cuộc hẹn với bệnh nhân.";
                        break;
                    case 15:
                        body = "Hợp đồng giữa bác sĩ và bệnh nhân " + fullNameSender + " đã có hiệu lực. Trong vòng 4 ngày kể từ ngày hợp đồng có hiệu lực,bác sĩ cần đưa ra một cuộc hẹn thăm khám hoặc một lịch đo sinh hiệu đầu tiên cho bệnh nhân.";
                        break;
                    case 16:
                        body = "Hợp đồng theo dõi giữa bác sĩ và bệnh nhân "+ fullNameSender+" đã bị hệ thống khóa lại do bác sĩ không đáp ứng được các yêu cầu từ hệ thống và bệnh nhân. Bác sĩ cần liên hệ với hệ thống và bệnh nhân để được giải quyết." ;
                        break;
                    case 17:
                        body = "Bệnh nhân " + fullNameSender + " trong danh sách theo dõi đã trở lại trạng thái bình thường.";
                        break; 
                    case 18:
                        body = "Hợp đồng theo dõi giữa bạn và bác sĩ " + fullNameSender + " đã hoàn thành. Nếu bạn muốn bác sĩ này tiếp tục theo dõi xin vui lòng gửi yêu cầu hợp đồng mới.";
                        break;
                    case 19:
                        body = "Hợp đồng theo dõi giữa bác sĩ và bệnh nhân " + fullNameSender + " đã hoàn thành.";
                        break;
                    case 20:
                        body = "Hợp đồng theo dõi giữa bạn và bác sĩ " + fullNameSender + " đã có hiệu lực.Trong vòng 4 ngày kể từ ngày hợp đồng có hiệu lực,bác sĩ sẽ đưa ra một cuộc hẹn thăm khám hoặc một lịch đo sinh hiệu đầu tiên cho bạn.";
                        break;
                    case 21:
                        body = "Hợp đồng theo dõi giữa bạn và bác sĩ " + fullNameSender + " đã bị khóa bởi hệ thống do trong vòng 4 ngày kể từ ngày hợp đồng có hiệu lực,bác sĩ đã không đưa ra bất kỳ y lệnh nào cho bạn.";
                        break;
                    case 22:
                        body = "Hôm nay bác sĩ có cuộc hẹn với bệnh nhân " + fullNameSender + ".Vui lòng kiểm tra để không nhỡ cuộc hẹn với bệnh nhân .";
                        break;
                    case 23:
                        body = "Hôm nay bạn có cuộc hẹn với bác sĩ " + fullNameSender + ".Vui lòng kiểm tra để không nhỡ cuộc hẹn với bác sĩ.";
                        break;
                    case 24:
                        body = "Bác sĩ đã thêm y lệnh vào hồ sơ sức khỏe của bệnh nhân " + fullNameSender + ".";
                        break;
                    case 25:
                        body = "Bác sĩ "+fullNameSender +" đã thêm y lệnh vào hồ sơ sức khỏe của bạn.";
                        break;
                    case 26:
                        body = "Bác sĩ "+fullNameSender+" đã hủy cuộc hẹn với bạn .Xin vui lòng kiểm tra lại lịch khám bệnh.";
                        break;
                    case 27:
                        body = "Bác sĩ "+fullNameSender+" "+ bodyCustom;
                        break;
                    case 28:
                        body = "Bác sĩ " + fullNameSender + " đã xác nhận hoàn thành cuộc hẹn với bạn.";
                        break;
                    case 29:
                        body = "Bệnh nhân " + fullNameSender + " cảm thấy bất thường và yêu cầu bác sĩ xem xét các giá trị sinh hiệu của họ.";
                        break;

                }
                return body;
            }
            return null;
        }

        public async Task<NotificationRespone> GetTimeLineOfPatient(int accountPatientId, int accountDoctorID, DateTime dateTime)
        {
            if(accountPatientId != 0 && accountDoctorID != 0 && dateTime != null)
            {
                // From Doctor To Patient
                var dToP = await _repo.GetDbSet().Where(x => x.AccountId == accountPatientId && x.AccountSendId == accountDoctorID && x.DateCreate.Date.Equals(dateTime.Date)).Include(x => x.NotificationType).ToListAsync();
                // From Patient to Doctor
                var pToD = await _repo.GetDbSet().Where(x => x.AccountId == accountDoctorID && x.AccountSendId == accountPatientId && x.DateCreate.Date.Equals(dateTime.Date)).Include(x => x.NotificationType).ToListAsync();
                var timeLine = dToP.Concat(pToD);
                if(timeLine.Any())
                {
                    var timeNow = DateTime.Now;
                    var respone = new NotificationRespone()
                    {
                        DateCreate = dateTime.Date.ToString("dd/MM/yyyy"),
                        Notifications = timeLine.Select(x => new NotificationRespone.Notification() {
                            NotificationType = x.NotificationTypeId,
                            Title = x.NotificationType.Title,
                            Body = GenarateBodySend(x.NotificationTypeId, x.AccountSendId.GetValueOrDefault(),x.BodyCustom).Result,
                            AppointmentId = x.AppointmentId,
                            MedicalInstructionId = x.MedicalInstructionId,
                            ContractId = x.ContractId,
                            TimeAgo = timeNow.Subtract(x.DateCreate).TotalMinutes, 
                            DateCreated = x.DateCreate.ToString("HH:mm")
                        }).OrderBy(x => x.TimeAgo).ToList()                       
                    };
                    return respone;
                }
            }
            return null;
        }

        public async Task<ICollection<object>> GetDateTimeHaveNotification(int accountPatientId, int accountDoctorID)
        {
            if(accountPatientId != 0 && accountDoctorID != 0)
            {
                // From P to D
                var dToP = await _repo.GetDbSet().Where(x => x.AccountSendId == accountPatientId && x.AccountId == accountDoctorID).Select(x => x.DateCreate.Date).Distinct().ToListAsync();
                // From D to P
                var pToD = await _repo.GetDbSet().Where(x => x.AccountSendId == accountDoctorID && x.AccountId == accountPatientId).Select(x => x.DateCreate.Date).Distinct().ToListAsync();
                var tmp = dToP.Concat(pToD).Distinct().ToList();
                if (tmp.Any())
                {
                    var respone = tmp.GroupBy(x => x.ToString("MM/yyyy")).Select(x => new 
                    {
                        Month = x.Key,
                        Days = x.Select(y => y.Day).OrderBy(y => y).ToList()
                    });
                    return respone.ToList<object>();
                }
            }
            return null;
        }
    

    }
}
