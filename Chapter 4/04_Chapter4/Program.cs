using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //fixed-time interval sequence
            var fixedTimeBasedSequence = Observable.Interval(TimeSpan.FromSeconds(1));

            //convert the message 
            //into time value
            var dateTimeSequence = fixedTimeBasedSequence
                .Select(v => DateTime.UtcNow);

            //filtered sequence of times with even second value
            var filteredSequence = dateTimeSequence.Where(dt => dt.Second % 2 == 0);

            Console.ReadLine();
        }
    }
}
