using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _10_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequence = Observable.Create<DateTime>(x =>
            {
                //let take some time before registering the new observer
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Registering observer on thread {0}...", Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(100);
                }

                //produce 10 messages
                for (int i = 0; i < 10; i++)
                {
                    x.OnNext(DateTime.Now);
                    Thread.Sleep(100);
                }

                //exit
                return Disposable.Empty;
            });

            //register two subscribers
            sequence.SubscribeOn(Scheduler.Default).Subscribe(x => Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x));
            sequence.SubscribeOn(Scheduler.Default).Subscribe(x => Console.WriteLine("{0} -> {1}", Thread.CurrentThread.ManagedThreadId, x));

            Console.ReadLine();
        }
    }
}
