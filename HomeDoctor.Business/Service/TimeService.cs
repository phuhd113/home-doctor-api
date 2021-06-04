using HomeDoctor.Business.IService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class TimeService : ITimeService
    {
        public struct SystemDate
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;
        };
        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool SetSystemTime(ref SystemDate sysDate);

        public async Task UpdateSystemTime(DateTime time,bool onLinux)
        {
            if(time != null)
            {
                if (onLinux)
                {
                    Console.WriteLine("Before Linux : " + DateTime.Now);
                    string cmdTimeLinux = "date -s "+"'"+time.ToString("dd MMM yyyy HH:mm:ss")+"'";
                    var process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "/bin/bash",
                            Arguments = $"-c \"{cmdTimeLinux}\"",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };
                    process.Start();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                    process.WaitForExit();
                }
                else
                {
                    Console.WriteLine("Before Window : " + DateTime.Now);                   
                    SystemDate systemDate = new SystemDate();
                    systemDate.Year = (ushort)time.Year;
                    systemDate.Month = (ushort)time.Month;
                    systemDate.Day = (ushort)time.Day;
                    systemDate.Hour = (ushort)time.Hour;
                    systemDate.Minute = (ushort)time.Minute;
                    systemDate.Millisecond = (ushort)time.Millisecond;

                    var result = SetSystemTime(ref systemDate);
                    if (result)
                    {
                        Console.WriteLine("After : " + DateTime.Now);
                        Console.WriteLine("After UtcNow :" + DateTime.UtcNow);
                    }
                }
            }
            
        }
        public async Task ResetSystemTime()
        {
            string cmdTimeLinux = "hwclock --hctosys";
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{cmdTimeLinux}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
        }
    }
}
