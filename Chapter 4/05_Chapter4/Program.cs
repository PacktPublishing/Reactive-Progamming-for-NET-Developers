using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {

            //two sourcing sequences of time-based values
            var sourceSequence1 = Observable.Interval(TimeSpan.FromSeconds(2)).Select(nr => DateTime.UtcNow);
            var sourceSequence2 = Observable.Interval(TimeSpan.FromSeconds(3)).Select(nr => DateTime.UtcNow);

            //a joined sequence of messages
            var joinedSequence = sourceSequence1.Join(sourceSequence2,
                v => Observable.Return(v).Delay(TimeSpan.FromMilliseconds(100)),
                v => Observable.Return(v).Delay(TimeSpan.FromMilliseconds(100)),
                (v1, v2) => new { fromSequence1 = v1, fromSequence2 = v2 });

            joinedSequence.Subscribe(x =>
            {
                Console.WriteLine("{0} / {1}", x.fromSequence1, x.fromSequence2);
            });

            Console.ReadLine();
        }
    }
}
