using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //a sourcing sequence
            var sourcingSequence = Observable.Interval(TimeSpan.FromSeconds(1)).Select(id => DateTime.UtcNow);

            //will flow messages for next 5 seconds
            var takeUntil = sourcingSequence.TakeUntil(DateTimeOffset.Now.AddSeconds(5));
            takeUntil.Subscribe(value =>
                {
                    Console.WriteLine("Until5Seconds: {0}", value);
                });

            var begin = DateTime.UtcNow;
            //will flow messages while in the
            //same minute of the begin
            var takeWhile = sourcingSequence.TakeWhile(x => begin.Minute == x.Minute);
            takeWhile.Subscribe(value =>
                {
                    Console.WriteLine("WhileSameMinute: {0}", value);
                });

            //skip messages for 5 seconds
            var skipUntil = sourcingSequence.SkipUntil(DateTimeOffset.Now.AddSeconds(5));
            skipUntil.Subscribe(value =>
                {
                    Console.WriteLine("SkipFor5Seconds: {0}", value);
                });

            //skip messages of the same minute
            var skipWhile = sourcingSequence.SkipWhile(x => begin.Minute == x.Minute);
            skipWhile.Subscribe(value =>
                {
                    Console.WriteLine("SkipSameMinute: {0}", value);
                });

            Console.ReadLine();
        }
    }
}
