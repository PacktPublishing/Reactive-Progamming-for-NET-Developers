using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _08_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            ////as simple task
            //var task = Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(1000);
            //    return DateTime.Now;
            //});

            ////a sequence to ack the task's result
            ////need using System.Reactive.Threading.Tasks
            //var ackSequence = task.ToObservable();
            
            ////some output
            //ackSequence.Subscribe(x => Console.WriteLine(x));

            //a cancellable sequence
            var fromDatabase = Observable.Create<DateTime>(o =>
            {
                //a cancellation token source for timeout
                var tks = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var token = tks.Token;

                //the cancellable task within the sequence
                return Task.Factory.StartNew(() =>
                            {
                    //run until cancel requested
                    while (!token.IsCancellationRequested)
                                    using (var cn = new SqlConnection(@"data source=(local);integrated security=true;"))
                                    using (var cm = new SqlCommand("select getdate()", cn))
                                    {
                                        Thread.Sleep(1000);
                                        cn.Open();
                            //read time from DB
                            o.OnNext((DateTime)cm.ExecuteScalar());
                                    }

                    //signal oncompleted
                    o.OnCompleted();

                    //returns a disposable subscription completed object
                    //with an OnCompleted callback
                    return Disposable.Create(() => Console.WriteLine("Killing subscription"));
                            }, token);
            });

            fromDatabase.Subscribe(x => Console.WriteLine(x));

            Console.ReadLine();
        }
    }
}
