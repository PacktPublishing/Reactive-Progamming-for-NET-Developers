using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random(DateTime.Now.GetHashCode());

            //an infinite message source of integer numbers
            //running at 10hz
            var source = Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Select(x => r.Next(1, 20));

            var contains = source.Contains(10)
                //we want message metadata             
                .Materialize();

            //some console output
            source.Subscribe(x => Console.WriteLine(x));
            contains.Subscribe(x => Console.WriteLine("FOUND: {0}", x));

            var any = source.Any(x => x == 10)
                //we want message metadata             
                .Materialize();
            any.Subscribe(x => Console.WriteLine("FOUND ANY: {0}", x));

            Console.ReadLine();
        }
    }
}
