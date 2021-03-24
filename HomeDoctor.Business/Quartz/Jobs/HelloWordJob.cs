using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class HelloWordJob : IJob
    {

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello World");
            return Task.CompletedTask;
        }
    }
}
