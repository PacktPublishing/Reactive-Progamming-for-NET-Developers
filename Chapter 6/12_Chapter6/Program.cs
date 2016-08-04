using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _12_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var job1 = Scheduler.Default.Schedule(OnJob1Executed))
            //    //job timeout
            //    Thread.Sleep(2000);

            //starts a job in absolute time
            using (var job2 = Scheduler.Default.Schedule(DateTimeOffset.Now.AddSeconds(1), () => Console.WriteLine("OK")))
                //job timeout
                Thread.Sleep(2000);

            //starts a job in relative time
            using (var job3 = Scheduler.Default.Schedule(TimeSpan.FromSeconds(10), () => Console.WriteLine("OK")))
                //job timeout
                //this job will never fire because its schedule is greater than how time timeout will grant
                Thread.Sleep(2000);

            //starts a job periodically
            using (var job4 = Scheduler.Default.SchedulePeriodic(TimeSpan.FromSeconds(1), () => Console.WriteLine("OK")))
                //timeout at 5 seconds
                Thread.Sleep(5000);

            Console.WriteLine("END");
            Console.ReadLine();
        }

        static void OnJob1Executed()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }

            Console.WriteLine();
            Console.WriteLine("JOB END");
        }
    }
}
