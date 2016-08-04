using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter3
{
    class Program
    {
        static void Main(string[] args)
        {
            ////a new sequence
            //var s = new Subject<string>();
            ////subscribe such new observer OnNext implementation
            //s.Subscribe(Console.WriteLine);

            ////some push value
            //s.OnNext("value1");
            //s.OnNext("value2");

            //var simpleSubject = new Subject<string>();
            //simpleSubject.OnNext("value1");
            //simpleSubject.OnNext("value2");
            //simpleSubject.Subscribe(Console.WriteLine);
            //simpleSubject.OnNext("value3");
            //simpleSubject.OnNext("value4");

            //var replaySubject = new ReplaySubject<string>();
            //replaySubject.OnNext("value1");
            //replaySubject.OnNext("value2");
            //replaySubject.Subscribe(Console.WriteLine);
            //replaySubject.OnNext("value3");
            //replaySubject.OnNext("value4");

var behaviorSubject = new BehaviorSubject<DateTime>(new DateTime(2001, 1, 1));
Thread.Sleep(1000);
//the default value will flow to the new subscriber
behaviorSubject.Subscribe(x => Console.WriteLine(x));
Thread.Sleep(1000);
//a new value will flow to the subscriber
behaviorSubject.OnNext(DateTime.Now);
Thread.Sleep(1000);
//this new subscriber will receive the last available message
//regardless is was not subscribing at the time the message arise
behaviorSubject.Subscribe(x => Console.WriteLine(x));
Thread.Sleep(1000);

            //var asyncSubject = new AsyncSubject<string>();
            //asyncSubject.OnNext("value1"); //this will be missed
            //asyncSubject.Subscribe(Console.WriteLine);
            //asyncSubject.OnNext("value2"); //this will be missed
            //asyncSubject.OnNext("value3"); //this will be routed once OnCompleted raised
            //Console.ReadLine();
            //asyncSubject.OnCompleted();

            //var mapper = new MapperSubject<string, double>(x => double.Parse(x));
            //mapper.Subscribe(x => Console.WriteLine("{0:N4}", x));
            //mapper.OnNext("4.123");
            //mapper.OnNext("5.456");
            //mapper.OnNext("7.90'?");
            //mapper.OnNext("9.432");

            Console.ReadLine();
        }
    }

    public sealed class MapperSubject<Tin, Tout> : ISubject<Tin, Tout>
    {
        readonly Func<Tin, Tout> mapper;
        public MapperSubject(Func<Tin, Tout> mapper)
        {
            this.mapper = mapper;
        }

        public void OnCompleted()
        {
            foreach (var o in observers.ToArray())
            {
                o.OnCompleted();
                observers.Remove(o);
            }
        }

        public void OnError(Exception error)
        {
            foreach (var o in observers.ToArray())
            {
                o.OnError(error);
                observers.Remove(o);
            }
        }

        public void OnNext(Tin value)
        {
            Tout newValue = default(Tout);
            try
            {
                //mapping statement
                newValue = mapper(value);
            }
            catch (Exception ex)
            {
                //if mapping crashed
                OnError(ex);
                return;
            }

            //if mapping succeded
            foreach (var o in observers)
                o.OnNext(newValue);
        }

        //all registered observers
        private readonly List<IObserver<Tout>> observers = new List<IObserver<Tout>>();
        public IDisposable Subscribe(IObserver<Tout> observer)
        {
            observers.Add(observer);
            return new ObserverHandler<Tout>(observer, OnObserverLifecycleEnd);
        }

        private void OnObserverLifecycleEnd(IObserver<Tout> o)
        {
            o.OnCompleted();
            observers.Remove(o);
        }

        //this class simply informs the subject that a dispose
        //has been invoked against the observer causing its removal
        //from the observer collection of the subject
        private class ObserverHandler<T> : IDisposable
        {
            private IObserver<T> observer;
            Action<IObserver<T>> onObserverLifecycleEnd;
            public ObserverHandler(IObserver<T> observer, Action<IObserver<T>> onObserverLifecycleEnd)
            {
                this.observer = observer;
                this.onObserverLifecycleEnd = onObserverLifecycleEnd;
            }

            public void Dispose()
            {
                onObserverLifecycleEnd(observer);
            }
        }
    }
}
