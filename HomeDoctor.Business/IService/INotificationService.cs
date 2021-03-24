using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface INotificationService
    {
        public Task<bool> InsertNotification(NotificationRequest notification);
        public Task<ICollection<NotificationRespone>> GetNotificationByAccountId(int accountId, bool onSystem);
        public Task<bool> UpdateStatusNotificationByNotiId(int notiId);
        public Task<ICollection<HistoryRespone>> GetHistoryByAccountId(int accountId);
    }
}
