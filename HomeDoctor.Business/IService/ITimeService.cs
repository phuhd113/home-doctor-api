using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface ITimeService
    {
        public Task UpdateSystemTime(DateTime time, bool onLinux);
        public Task ResetSystemTime();
    }
}
