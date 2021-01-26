using HomeDoctor.Business.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDoctor.Api.Signalr
{
    public interface IHubClient
    {
        public Task BroadCastScanQR(PatientInformation patient);
    }
}
