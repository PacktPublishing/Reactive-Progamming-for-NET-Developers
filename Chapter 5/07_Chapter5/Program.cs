using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1))
                //stops after 5 seconds
                .TakeUntil(Observable.Return(0).Delay(TimeSpan.FromSeconds(5)));

            source.Subscribe(x => Console.WriteLine(x));

            //log the completion of the source
            source.Finally(() => Console.WriteLine("END"))
                //force the Finally sequence to 
                //start working by registering
                //an empty subscriber
                .Subscribe();

            Console.ReadLine();
        }
    }
}
