using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _09_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            //var loopBasedSequence = Observable.Create<DateTime>(x =>
            //{
            //    while (true)
            //    {
            //        Console.WriteLine("{0} -> Yielding new value...", Thread.CurrentThread.ManagedThreadId);
            //        x.OnNext(DateTime.Now);
            //        Thread.Sleep(1000);
            //    }
            //    return Disposable.Empty;
            //});

            //loopBasedSequence.Subscribe(x => Console.WriteLine("-> {0}", x));

            //var timerBasedSequence = Observable.Interval(TimeSpan.FromSeconds(1))
            //.Select(x =>
            //{
            //    Console.WriteLine("{0} -> Yielding new value...", Thread.CurrentThread.ManagedThreadId);
            //    return DateTime.Now;
            //});

            //timerBasedSequence.Subscribe(x => Console.WriteLine("-> {0}", x));

            //using System.Reactive.Concurrency
            //var scheduler = Scheduler.Default;

            var loopBasedSequence = Observable.Create<DateTime>(x =>
            {
                while (true)
                {
                    //Console.WriteLine("{0} -> Yielding new value...", Thread.CurrentThread.ManagedThreadId);
                    x.OnNext(DateTime.Now);
                    Thread.Sleep(1000);
                }
                return Disposable.Empty;
            });

            loopBasedSequence.SubscribeOn(NewThreadScheduler.Default).Subscribe(x => Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x));
            loopBasedSequence.SubscribeOn(NewThreadScheduler.Default).Subscribe(x => Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x));
            loopBasedSequence.SubscribeOn(NewThreadScheduler.Default).Subscribe(x => Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x));

            Console.ReadLine();
        }
    }
}
