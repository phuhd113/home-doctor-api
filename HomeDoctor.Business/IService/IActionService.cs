using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IActionService
    {
        public Task<bool> CheckActionFirstTime(int contractId, bool? appointmentFirst, bool? vitalSignScheduleFirst);

        public Task<bool> UpdateActionFirstTime(int contractId, bool? appointmentFirst, bool? vitalSignScheduleFirst);
    }
}
