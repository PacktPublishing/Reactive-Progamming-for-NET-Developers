using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _06_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            //var sequence = Observable.Create<int>(observer =>
            //{
            //    //a task is required for all time consuming activities
            //    Task.Factory.StartNew(() =>
            //                {
            //                    for (int i = 0; i < 100; i++)
            //                    {
            //                        //lot of CPU time
            //                        Thread.SpinWait(10000000);
            //                        //diagnostic output
            //                        Debug.WriteLine(string.Format("Flowing value: {0}", i));
            //                        //flow out a message
            //                        observer.OnNext(i);
            //                    }

            //                    observer.OnCompleted();
            //                });

            //    return Disposable.Empty;
            //});

            //var sequence = Observable.Create<int>(observer =>
            //{
            //    var cts = new CancellationTokenSource();
            //    var token = cts.Token;

            //    var task = Task.Factory.StartNew(() =>
            //    {
            //        for (int i = 0; i < 100; i++)
            //        {
            //            //raise an exception to stop thread's execution on task cancellation request
            //            token.ThrowIfCancellationRequested();

            //            //lot of CPU time
            //            Thread.SpinWait(10000000);
            //            //diagnostic output
            //            Debug.WriteLine(string.Format("Flowing value: {0}", i));
            //            //flow out a message
            //            observer.OnNext(i);
            //        }
            //    }, token);

            //    ////executes the following action at the subscription disposal
            //    //return Disposable.Create(() => cks.Cancel());

            //    //raise the token cancellation at the disposal
            //    return new CancellationDisposable(cts);
            //});

            var sequence = Observable.Create<int>(observer =>
            {
                var thread = new Thread(new ThreadStart(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        //lot of CPU time
                        Thread.SpinWait(10000000);
                        //diagnostic output
                        Debug.WriteLine(string.Format("Flowing value: {0}", i));

                        Thread.BeginCriticalRegion();
                        //don't kill me here
                        Thread.EndCriticalRegion();

                        //flow out a message
                        observer.OnNext(i);
                    }
                }));

                thread.Start();

                //executes the following action at the subscription disposal
                return Disposable.Create(() => thread.Abort());
            });

            var subscription = sequence.Subscribe(x => Console.WriteLine(x));

            //wait 5 seconds
            Thread.Sleep(1000);
            //kill the subscription
            subscription.Dispose();


            Console.ReadLine();
        }
    }
}
