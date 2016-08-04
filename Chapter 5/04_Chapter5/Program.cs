using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace _04_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random(DateTime.Now.GetHashCode());

            var stopperSequence = new Subject<bool>();

            //an infinite message source of integer numbers
            //running at 10hz
            var source = Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Select(x => r.Next(1, 20))
                //take only until we press RETURN
                .TakeUntil(stopperSequence);

            source.Subscribe(x => Console.WriteLine(x));

            var all = source.All(x => x < 18)
                //we want message metadata             
                .Materialize();

            all.Subscribe(x => Console.WriteLine("FOUND ALL: {0}", x));

            //wait until user press RETURN
            Console.ReadLine();
            //notify the stop message
            stopperSequence.OnNext(true);
            //wait again to see the result
            Console.ReadLine();
        }
    }
}
