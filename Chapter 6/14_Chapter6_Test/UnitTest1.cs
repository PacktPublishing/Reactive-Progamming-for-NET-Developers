using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive;
using Microsoft.Reactive.Testing;
using System.Reactive.Concurrency;

namespace _14_Chapter6_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var scheduler = new TestScheduler();

            //a cold sequence
            var sequence = scheduler.CreateColdObservable<int>(
                //some recorded message
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(1).Ticks, Notification.CreateOnNext(10)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnNext(20)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(3).Ticks, Notification.CreateOnNext(30)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(4).Ticks, Notification.CreateOnNext(40)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(5).Ticks, Notification.CreateOnNext(50)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(6).Ticks, Notification.CreateOnNext(60)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(7).Ticks, Notification.CreateOnCompleted<int>())
            );

            //a new testable observer
            var observer1 = scheduler.CreateObserver<int>();

            //subscribe the observer at a given virtual time
            scheduler.Schedule(TimeSpan.FromSeconds(2), () => sequence.Subscribe(observer1));

            //play the record
            scheduler.Start();

            foreach (var m in observer1.Messages)
            {
                var time = m.Time;
                //available only for OnNext messages
                //var value = m.Value.Value;
                //var exception = m.Value.Exception;
                //var kind = m.Value.Kind;
                //var hasValue = m.Value.HasValue;

                Console.WriteLine("{0}", m);
            }

            //unit testing

            observer1.Messages.AssertEqual(
                //same messages
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(1).Ticks + TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnNext(10)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(2).Ticks + TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnNext(20)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(3).Ticks + TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnNext(30)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(4).Ticks + TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnNext(40)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(5).Ticks + TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnNext(50)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(6).Ticks + TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnNext(60)),
                new Recorded<Notification<int>>(TimeSpan.FromSeconds(7).Ticks + TimeSpan.FromSeconds(2).Ticks, Notification.CreateOnCompleted<int>())
            );
        }
    }
}
