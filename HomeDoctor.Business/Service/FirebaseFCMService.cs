using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class FirebaseFCMService : IFirebaseFCMService
    {
        private readonly IRepositoryBase<NotificationType> _repoNotiType;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly INotificationService _serNoti;
        private readonly IUnitOfWork _uow;

        private const string ServerKeyWeb = "AAAARNhYCi8:APA91bFATUCuXrnfaw1e7xrJUui4U5aljEgmHo2YA7Nf9Wpjq_9NTHtU4w6dXZArVgz__iiB_V5URbix8kYslCpKHOEV_1sJW0PFlekAA4gBzGqoXdVS-oOO_riF-0zq8Vl_pbZVepHF";       
        private const string ServerKeyIos = "AAAA4vVZ6E4:APA91bFqFnL5dFUv2eXien727gsM6yj8yUObiZtFiYgiw_xd6i3h_0j1CSZePuVwIsyvSwYcxdPYo7TDoqIUbNPgxz3zEauo1MEJw8I6LRlxT-MP2057yuIvkwsRZiBUbnXlBr53SwA-";
        private const string SenderIdIos = "974778918990";
        private const string SenderIdWeb = "295687424559";

        public FirebaseFCMService(INotificationService serNoti, IUnitOfWork uow)
        {
            _serNoti = serNoti;
            _uow = uow;
            _repoNotiType = _uow.GetRepository<NotificationType>();
            _repoAccount = _uow.GetRepository<Account>();
        }

        public async Task<bool> SaveToken(int accountId, string token)
        {
            if (accountId != 0)
            {
                var account = await _repoAccount.GetDbSet().Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
                if (account != null)
                {
                    account.FireBaseToken = token;
                    var tmp = await _repoAccount.Update(account);
                    if (tmp)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }             
            }
            return false;
        }

        //deviceType = 1 . Notification Ios.
        //deviceType = 2 . Notification Web.
        //deviceType = 3 . Notification Android.

        public async Task PushNotification(int deviceType, int senderAccountId, int accountId, int notiTypeId, int? contractId, int? medicalInstructionId, int? appointmentId, string? bodyCustom)
        {
            if (accountId != 0)
            {
                var tokenDevice = await _repoAccount.GetDbSet().Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
                if (tokenDevice.FireBaseToken != null)
                {
                    //Config request
                    WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    webRequest.Method = "POST";
                    webRequest.UseDefaultCredentials = true;
                    webRequest.ContentType = "application/json";

                    //Config key 
                    if (deviceType == 1)
                    {
                        webRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKeyIos));
                        webRequest.Headers.Add(string.Format("Sender: id={0}", SenderIdIos));
                    }
                    if (deviceType == 2)
                    {
                        webRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKeyWeb));
                        webRequest.Headers.Add(string.Format("Sender: id={0}", SenderIdWeb));
                    }

                    var payload = new object();
                    //config data send 
                    if (notiTypeId != 0)
                    {                        
                        var noti = await _repoNotiType.GetById(notiTypeId);
                        payload = new
                        {
                            to = tokenDevice.FireBaseToken,
                            notification = new
                            {
                                title = noti.Title,
                                body = await _serNoti.GenarateBodySend(noti.NotificationTypeId, senderAccountId,bodyCustom),
                            },
                            data = new
                            {
                                notiTypeId,
                                contractId,
                                medicalInstructionId,
                                appointmentId
                            }
                        };
                        if(notiTypeId == 12)
                        {

                        }
                    }
                    // Parse Json
                    var postData = JsonConvert.SerializeObject(payload).ToString();
                    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    webRequest.ContentLength = byteArray.Length;
                    using (Stream dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = webRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                    {
                                        string sResponseFromServer = tReader.ReadToEnd();
                                        Console.WriteLine(sResponseFromServer);
                                    }
                            }
                        }
                    }
                }
            }
        }
    }
}
