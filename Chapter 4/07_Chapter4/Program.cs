using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _07_Chapter4
{
class Program
{
    static void Main(string[] args)
    {
        //this sequence must complete by 5 seconds from now
        var absoluteTimeoutSequence = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(id => DateTime.UtcNow)
            .Timeout(DateTimeOffset.Now.AddSeconds(5));

        absoluteTimeoutSequence.Subscribe(new ConsoleObserver());

        Console.WriteLine("Press RETURN to start the following example");
        Console.ReadLine();

        //this sequence's messages must flow
        //by 2 seconds
        var relativeTimeoutSequence = Observable.Create<DateTime>(newObserver =>
            {
                Console.WriteLine("Registering observer...");
                Console.WriteLine("Starting message flow...");

                //handle the new subscriber message flow
                Task.Factory.StartNew(() =>
                {
                    //the message flow will slow down until timeout
                    int i = 100;
                    while (true)
                    {
                        newObserver.OnNext(DateTime.UtcNow);
                        //the delay will increase each iteration
                        Thread.Sleep(i += 100);
                    }
                }, TaskCreationOptions.PreferFairness);

                return new Action(() =>
                {
                    Console.WriteLine("Completed");
                });
            })
            .Timeout(TimeSpan.FromSeconds(2));

        relativeTimeoutSequence.Subscribe(new ConsoleObserver());

        Console.ReadLine();
    }
}

public class ConsoleObserver : IObserver<DateTime>
{
    public void OnCompleted()
    {
        Console.WriteLine("Observer completed!");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine("Observer error: {0}", error);
    }

    public void OnNext(DateTime value)
    {
        Console.WriteLine("{0}", value);
    }
}

}
