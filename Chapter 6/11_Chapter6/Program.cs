using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _11_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
Console.WriteLine("Main thread: {0}", Thread.CurrentThread.ManagedThreadId);

//numeric sequence
var sequence = Observable.Range(1, 10, Scheduler.Default);

//observers
sequence.Subscribe(x => Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x));
sequence.Subscribe(x => Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x));

            //var sequence = Observable.Create<DateTime>(o =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        o.OnNext(DateTime.Now);
            //        Thread.Sleep(100);
            //    }

            //    o.OnCompleted();
            //    return Disposable.Create(() => Console.WriteLine("Killing observer..."));
            //});

            //var scheduler = Scheduler.Default;
            //scheduler.Schedule(() =>
            //{
            //    Console.WriteLine("CIAO");
            //});

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
