    using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //a sourcing sequence
            var sourcingSequence = Observable.Interval(TimeSpan.FromSeconds(1)).Select(id => DateTime.UtcNow);

            //sequence partitioning by seconds
            var partitions = sourcingSequence.GroupBy(x => Math.Floor(x.Second / 10d));

            ////register the partition per group key
            //partitions.Subscribe(partition =>
            //    {
            //        Console.WriteLine("Registering observer for: {0}", partition.Key);

            //        //register the observer per partition
            //        partition.Subscribe(value =>
            //            {
            //                Console.WriteLine("partition {0}: {1}", partition.Key, value);
            //            });
            //    });

            //register the partition per group key
            //without nested sequences
            partitions
                //transform inner groups into new objects
                //containing the key and the value altogether
                .SelectMany(group => group.Select(x => new { key = group.Key, value = x }))
                .Subscribe(msg => Console.WriteLine("partition {0}: {1}", msg.key, msg.value));


            Console.ReadLine();
        }
    }
}
