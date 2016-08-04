using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _03_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new CpuThrottlingScheduler() { CpuLimitPercentage = 50 };

            //a simple looping sequence
            var sequence = Observable.Range(0, 50, scheduler);

            //a huge observer list
            for (int i = 0; i < 10; i++)
                sequence.Subscribe(x =>
                {
                    Thread.SpinWait(100000000);
                    Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x);
                });

            Console.ReadLine();
        }
    }

    /// <summary>
    /// Enqueues unit of works only if the current CPU time is lower than the
    /// specified limit.
    /// </summary>
    public class CpuThrottlingScheduler : IScheduler, IDisposable
    {
        public int CpuLimitPercentage { get; set; } = 80;
        public DateTimeOffset Now { get; private set; }

        private static PerformanceCounter cpuTimeCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            while (true)
            {
                //checks the CPU time
                var cpu = cpuTimeCounter.NextValue();
                if (cpu >= CpuLimitPercentage)
                    Thread.Sleep(200);
                else
                    break;
            }

            //once the CPU time is lower than the limit
            //enqueue the job on the thread pool
            new Thread(new ThreadStart(() => action(this, state))).Start();
            Now += TimeSpan.FromTicks(1);

            return Disposable.Empty;
        }

        /// <summary>
        /// Not supported! Will be scheduled immediately
        /// </summary>
        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return Schedule<TState>(state, action);
        }

        /// <summary>
        /// Not supported! Will be scheduled immediately
        /// </summary>
        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return Schedule<TState>(state, action);
        }

        public void Dispose()
        {
            cpuTimeCounter.Dispose();
        }
    }
}
