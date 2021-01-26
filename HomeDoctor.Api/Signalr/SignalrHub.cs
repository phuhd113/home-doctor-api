using HomeDoctor.Business.IService;
using HomeDoctor.Business.ViewModel;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDoctor.Api.Signalr
{
    public class SignalrHub : Hub<IHubClient>
    {
        public async Task BroadCastScanQR(PatientInformation patient)
        {
            await Clients.All.BroadCastScanQR(patient);
        }
    }
}
