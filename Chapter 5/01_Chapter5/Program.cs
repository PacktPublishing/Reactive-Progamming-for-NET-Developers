using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace _01_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            //a simple sequence of DateTime
            var sequence1 = new Subject<DateTime>();

            //a console observer
            sequence1.Subscribe(x => Console.WriteLine("{0}", x));

            //a tracing sequence
            //of materialized notifications
            IObservable<Notification<DateTime>> tracingSequence = sequence1.Materialize();
            tracingSequence.Subscribe(notification =>
            {
                //this represents the operation
                Console.WriteLine("Operation: {0}", notification.Kind);

                //has a value
                if (notification.HasValue)
                                Console.WriteLine("Value: {0}", notification.Value);

                //has an exception
                else if (notification.Exception != null)
                    Console.WriteLine("Exception: {0}", notification.Exception);
            });

//a dematerialized sequence
var valueSequence = tracingSequence.Dematerialize();
//a console observer
valueSequence.Subscribe(x => Console.WriteLine("D: {0}", x));

            //flows a new value
            sequence1.OnNext(DateTime.Now);

            //flows the oncomplete message
            sequence1.OnCompleted();

            Console.ReadLine();
        }
    }
}
