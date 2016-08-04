using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _14_Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            ////the sourcing sequence of errors or completed messages
            //var sourcingSequence = Observable.Throw<object>(new Exception("Test")); // .Empty(new object())

            ////a sequence able to handle only errors or completed messages
            //var ignoredElements = sourcingSequence.IgnoreElements();
            //ignoredElements.Subscribe(new ConsoleObserver());

            ////the repeating sequence
            //var repeatFor2Times = sourcingSequence.Repeat(2);
            //repeatFor2Times.Subscribe(value => Console.WriteLine("Value: {0}", value));


            //var r = new Random(DateTime.Now.GetHashCode());
            ////a randomic value sequence
            //var sourcingSequence = Observable.Range(1, 5)
            //    //slow down
            //    .Select(i => { Thread.Sleep(1000); return i; })
            //    //take the actual time
            //    .Select(i => r.Next());

            ////multiple subscriptions causing different
            ////values being printed onto the console
            //sourcingSequence.Subscribe(value => Console.WriteLine("Observer#1: {0}", value));
            //sourcingSequence.Subscribe(value => Console.WriteLine("Observer#2: {0}", value));
            //sourcingSequence.Subscribe(value => Console.WriteLine("Observer#3: {0}", value));



            ////the sourcing sequence
            //var publishedSequence = Observable.Interval(TimeSpan.FromSeconds(0.5))
            //    .Select(i => DateTime.Now)
            //    .Publish();

            ////attach subscribers before connecting the publisher
            //publishedSequence.Subscribe(value => Console.WriteLine("Observer#1: {0}", value));
            //publishedSequence.Subscribe(value => Console.WriteLine("Observer#2: {0}", value));
            //publishedSequence.Subscribe(value => Console.WriteLine("Observer#3: {0}", value));

            //while (true)
            //{
            //    Console.WriteLine("Press RETURN to connect the published sequence");
            //    Console.ReadLine();
            //    using (var connected = publishedSequence.Connect())
            //    {
            //        Console.WriteLine("Press RETURN to quit the connection");
            //        Console.ReadLine();
            //    }
            //    //now we disconnected from the published sequence 
            //}

            ////the sourcing sequence
            //var publishedSequence = Observable.Interval(TimeSpan.FromSeconds(0.5))
            //    .Select(i => DateTime.Now)
            //    .Publish()
            //    .RefCount();

            //while (true)
            //{
            //    Console.WriteLine("Press return to subscribe");
            //    Console.ReadLine();
            //    using (var subscription = publishedSequence.Subscribe(value => Console.WriteLine("Observer: {0}", value)))
            //    {
            //        Console.WriteLine("Press return to unsubscribe");
            //        Console.ReadLine();
            //    }
            //    //now we disconnected from the published sequence 
            //}

            ////the sourcing sequence
            //var publishedSequence = Observable.Interval(TimeSpan.FromSeconds(0.5))
            //    .Select(i => DateTime.Now)
            //    .Take(5)
            //    .PublishLast();

            //publishedSequence.Subscribe(value => Console.WriteLine("Last: {0}", value));
            //publishedSequence.Connect();

            ////the sourcing sequence will fire
            ////for 5 seconds
            //var publishedSequence = Observable.Interval(TimeSpan.FromSeconds(1))
            //    .Select(i => DateTime.Now)
            //    .Take(5)
            //    .Replay(10);

            ////we wait for 2 seconds
            //Thread.Sleep(2000);

            ////now we connect the subscriber that will
            ////recover all messages thanks to the replay behaviour
            //publishedSequence.Subscribe(value => Console.WriteLine("Value: {0}", value));
            //publishedSequence.Connect();

            //the sourcing sequence
            var sourcingSequence = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(i => DateTime.Now);

            //the subject that will route messages
            var multicastingSubject = new Subject<DateTime>();

            //the publisher sequence
            var multicastSequence = sourcingSequence.Multicast(multicastingSubject);

            //subscribers
            multicastSequence.Subscribe(value => Console.WriteLine("Observer#1: {0}", value));
            multicastSequence.Subscribe(value => Console.WriteLine("Observer#2: {0}", value));

            //connect the publisher sequence
            multicastSequence.Connect();

            Console.ReadLine();
        }
    }

    public class ConsoleObserver : IObserver<object>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Observer completed!");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Observer error: {0}", error);
        }

        public void OnNext(object value)
        {
            Console.WriteLine("{0}", value);
        }
    }
}
