using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Business.Quartz
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public Type JobType { get;}
        public string CronExpression { get; }

    }
}
