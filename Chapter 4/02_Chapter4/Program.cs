using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //a ranged sequence
            var range = Observable.Range(0, 1000);
            //an observer will get values
            //anytime it will subscribe
            range.Subscribe(value =>
                {
                    Console.WriteLine("range -> {0}", value);
                });


            //a reactive For statement
            //similar to for(int i=0;i<10;i++)
            var generated = Observable.Generate<int, DateTime>(0, i => i < 10, i => i + 1, i =>
                {
                    return new DateTime(2016, 1, 1).AddDays(i);
                });
            generated.Subscribe(value =>
                {
                    Console.WriteLine("generated -> {0}", value);
                });



            //this sequence produces a message per second
            var sequence = Observable.Interval(TimeSpan.FromSeconds(1));
            sequence.Subscribe(ObserverOnNext);


            Console.ReadLine();
        }

        private static void ObserverOnNext(long obj)
        {
            Console.WriteLine("{0} -> {1}", obj, DateTime.Now);
        }
    }
}
