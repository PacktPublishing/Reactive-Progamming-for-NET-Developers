using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            //two random generators
            //without the randomic initial seeed
            var r1 = new Random();
            var r2 = new Random();

            //two infinite message source of integer numbers running at 1hz
            var source1 = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                .Select(x => r1.Next(1, 20));

            var source2 = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                .Select(x => r2.Next(1, 20));

            var identical = source1.SequenceEqual(source2)
                .Materialize();

            source1.Subscribe(x => Console.WriteLine("1: {0}", x));
            source2.Subscribe(x => Console.WriteLine("2: {0}", x));
            identical.Subscribe(x => Console.WriteLine("Equals: {0}", x));

            Console.ReadLine();
        }
    }
}
