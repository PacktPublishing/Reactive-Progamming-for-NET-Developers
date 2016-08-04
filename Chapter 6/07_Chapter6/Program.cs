using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            //an infinite sequence
            var sequence = Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => DateTime.Now);

            //the event wrapper
            var eventWrapper = sequence.ToEvent();

            //register the event handler
            eventWrapper.OnNext += x => Console.WriteLine("{0}", x);

            Console.ReadLine();
        }
    }
}
