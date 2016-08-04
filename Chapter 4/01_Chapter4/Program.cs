using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            //an empty sequence ended with a completed message
            IObservable<string> s2ended = Observable.Empty<string>();
            //an infinite sequence
            IObservable<string> s2infinite = Observable.Never<string>();


            //a sequence from a value
            IObservable<double> s3 = Observable.Return(40d);

            //a faulty sequence
            IObservable<DateTime> s4 = Observable.Throw<DateTime>(new Exception("Now"));

var s5 = Observable.Create<DateTime>(observer =>
    {
        Console.WriteLine("Registering subscriber {0} of type {1}", observer.GetHashCode(), observer);
        //here you can handle by hand your observer interaction logic
        Task.Factory.StartNew(() =>
            {
                //some (time based) message
                for (int i = 0; i < 10; i++)
                {
                    observer.OnNext(DateTime.Now);
                    Thread.Sleep(1000);
                }
                //end of observer life
                observer.OnCompleted();
            });
        return Disposable.Create(() => Console.WriteLine("Disposing..."));
    });

//subscribe an anonymous observer
var disposableObserver = s5.Subscribe(d =>
    {
        Console.WriteLine("OnNext : {0}", d);
    });

//wait some time and than press RETURN
//to dispose the observer
Console.ReadLine();

disposableObserver.Dispose();

Console.ReadLine();
        }
    }
}
