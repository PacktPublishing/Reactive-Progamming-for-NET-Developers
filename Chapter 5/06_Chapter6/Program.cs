using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random(DateTime.Now.GetHashCode());

            //an infinite message source of integer numbers running at 10hz
            var source1 = Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Select(x => r.Next(1, 20))
                //raise an exception on high values
                .Select(x =>
                {
                    if (x >= 19)
                        throw new ArgumentException("Value too high");
                    else
                        return x;
                })
                //a single shared subscription available to all following subscribers
                .Publish();

            //enable the connectable sequence
            source1.Connect();

            //an infinite message source of integer numbers running at 1hz
            var source2 = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                .Select(x => r.Next(20, 40));

            //a new sequence that continues with source2 when source1 raise an error
            var output = source1.Catch(source2)
                //we want message metadata for out testing purpose
                .Materialize();

            //output all values
            output.Subscribe(x => Console.WriteLine(x));
            //output when source1 raise the error
            source1.Materialize()
                .Where(x => x.Kind == System.Reactive.NotificationKind.OnError)
                .Subscribe(x => Console.WriteLine("Error: {0}", x.Exception));

            //specific exception handling
            var output2 = source1.Catch<int, ArgumentException>(exType => source2);

            //handle excetion and stop flowing messages
            var output3 = source1.Catch(Observable.Empty<int>());

            Console.ReadLine();
        }
    }
}
