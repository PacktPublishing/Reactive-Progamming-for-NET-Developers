using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _06_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //two sourcing sequences of time values

            var sourcingSequence1 = Observable.Interval(TimeSpan.FromSeconds(2))
                .Select(id => DateTime.UtcNow);

            var sourcingSequence2 = Observable.Interval(TimeSpan.FromSeconds(3))
                .Select(id => DateTime.UtcNow);


            //a selection function to choose
            //which sourcing sequence to use
            var mustUseTheFirstSequenceSelector = new Func<bool>(() =>
                {
                    var isFirst = DateTime.UtcNow.Second % 2 == 0;
                    Console.WriteLine("IsFirst: {0}", isFirst);
                    return isFirst;
                });

            //a conditional sequence of values from  
            // the first or the second sourcing sequence
            var conditionalSequence = Observable.If(mustUseTheFirstSequenceSelector, sourcingSequence1, sourcingSequence2);

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Subscribing new observer...");
                conditionalSequence.Subscribe(x =>
                    {
                        Console.WriteLine("{0}", x);
                    });
                Thread.Sleep(1000);
            }

            Console.ReadLine();
        }
    }
}
