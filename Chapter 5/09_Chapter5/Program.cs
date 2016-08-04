using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _09_Chapter5
{
    class Program
    {

static void Main(string[] args)
{
    var source = Observable.Interval(TimeSpan.FromSeconds(1))
        .Select(x => DateTime.Now)
        .Take(5)
        .Select(x =>
        {
            if (x.Second % 10 == 0)
                throw new ArgumentException();

            return x;
        })
        .Do(OnNext, OnError, OnCompleted)
                
        .Catch(Observable.Empty<DateTime>());

    //starts the source
    source.Subscribe();

    Console.ReadLine();
}

private static void OnError(Exception ex)
{
    Console.WriteLine("-> {0}", ex.Message);
}

private static void OnCompleted()
{
    Console.WriteLine("-> END");
}

private static void OnNext(DateTime obj)
{
            Thread.Sleep(4000);
    Console.WriteLine("-> {0}", obj);
}
    }
}
