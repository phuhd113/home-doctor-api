using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IPersonalHealthRecordService
    {
        public Task<bool> UpdateSmartWatchConnect(int patientId,bool isConnected);

    }
}
