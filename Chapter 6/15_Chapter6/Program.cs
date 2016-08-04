using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Microsoft.Reactive.Testing;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{0} -> Playing...", DateTime.Now);

            //a sourcing sequence
            var sequence = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

            var trace = sequence
                //marks each message with a timestamp
                .Timestamp()
                //route messages into a list of timestamped messages
                .ToList()
                //materialize the list when the sequence completes
                //and return only the list
                .Wait();

            //a scheduler for historical records
            var scheduler = new HistoricalScheduler();

            Console.WriteLine("{0} -> Replaying...", DateTime.Now);

            //generate a new sequence from a collection
            var replay = Observable.Generate(
                //the enumerator to read values from
                trace.GetEnumerator(),
                //the condition to check until False
                x => x.MoveNext(),
                //the item
                x => x,
                //the item's value
                x => x.Current.Value,
                //the item's virtual time
                x => x.Current.Timestamp,
                //the scheduler 
                scheduler);

            //some output
            replay.Subscribe(x => Console.WriteLine("{0} -> {1}", scheduler.Now, x));

            //play the record
            scheduler.Start();

            Console.ReadLine();
        }
    }
}
