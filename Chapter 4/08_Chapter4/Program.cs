using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //a sourcing sequence
            var sourcingSequence = Observable.Interval(TimeSpan.FromSeconds(1)).Select(id => DateTime.UtcNow);
            sourcingSequence.Subscribe(value =>
                {
                    Console.WriteLine("{0}", value);
                });

            //a sequence recording the time interval of the sourcing sequence
            var diagnosticSequence = sourcingSequence.TimeInterval();
            diagnosticSequence.Subscribe(interval =>
                {
                    Debug.WriteLine(string.Format("Message flowing in {0:N0}ms", interval.Interval.TotalMilliseconds));
                });

            var diagnosticSequence2 = sourcingSequence.Timestamp();
            diagnosticSequence2.Subscribe(new MessageTimeStampLogger());

            Console.ReadLine();
        }
    }

    public class MessageTimeStampLogger : IObserver<Timestamped<DateTime>>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Observer completed!");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Observer error: {0}", error);
        }

        public void OnNext(Timestamped<DateTime> value)
        {
            Debug.WriteLine(string.Format("{0} -> Now flowing: {1}", value.Timestamp, value.Value));
        }
    }

}
