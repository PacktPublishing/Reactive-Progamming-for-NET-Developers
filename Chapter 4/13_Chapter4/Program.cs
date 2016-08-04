using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //a sourcing sequence of 2 messages per second
            var sourcingSequence = Observable.Interval(TimeSpan.FromSeconds(0.5))
                //a transformation into DateTime
                //skipping milliseconds/nanoseconds
                .Select(id => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second))
                //we take messages only for 5 seconds
                .TakeUntil(DateTimeOffset.Now.AddSeconds(5));

            //the maxby sequence
            var maxBySequence = sourcingSequence.MaxBy(d => d.Ticks);
            maxBySequence.Subscribe(ordered =>
            {
                foreach (var value in ordered)
                    Console.WriteLine("MaxBy: {0}", value);
            });

            //the minby sequence
            var minBySequence = sourcingSequence.MinBy(d => d.Ticks);
            minBySequence.Subscribe(ordered =>
            {
                foreach (var value in ordered)
                    Console.WriteLine("MinBy: {0}", value);
            });

            Console.ReadLine();
        }
    }

    public class MyType : IComparable
    {
        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            return this.Name.CompareTo(obj);
        }
    }
}
