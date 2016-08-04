using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random(DateTime.Now.GetHashCode());

            //an infinite message source
            var source = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(i =>
                {
        //here messages went created

        //let's add some randomic delay
        Thread.Sleep(r.Next(100, 1000));

                    return DateTime.Now;
                });

            //the timing tracing sequence
            var timingsTracingSequence = source.TimeInterval();

            //valid messages
            var validMessagesSequence = timingsTracingSequence.Where(x => x.Interval.TotalMilliseconds <= 1200);
            //var exceeding messages
            var exceedingMessagesSequence = timingsTracingSequence.Where(x => x.Interval.TotalMilliseconds > 1200);

            //some console output
            validMessagesSequence.Subscribe(x => Console.WriteLine(x.Value));
            exceedingMessagesSequence.Subscribe(x => Console.WriteLine("Exceeding timing limits for: {0} ({1:N0}ms)", x.Value, x.Interval.TotalMilliseconds));

            Console.ReadLine();
        }
    }
}
