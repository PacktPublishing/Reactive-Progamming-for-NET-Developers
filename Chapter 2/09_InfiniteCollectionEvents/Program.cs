using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _09_InfiniteCollectionEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            //we create a variable containing the enumerable
            //this does not trigger item retrieval
            //so the enumerator does not begin flowing datas
            var enumerable = EnumerateValuesFromSomewhere();

            using (var observable = new EnumerableObservable(enumerable))
            using (var observer = observable.Subscribe(new ConsoleStringObserver()))
            {
                //wait for 2 seconds than exit
                Thread.Sleep(2000);
            }

            Console.WriteLine("Press RETURN to EXIT");
            Console.ReadLine();
        }

        static IEnumerable<string> EnumerateValuesFromSomewhere()
        {
            var random = new Random(DateTime.Now.GetHashCode());
            while (true) //forever
            {
                //returns a random integer number as string
                yield return random.Next().ToString();
                //some throttling time
                Thread.Sleep(100);
            }
        }
    }

    public sealed class EnumerableObservable : IObservable<string>, IDisposable
    {
        private readonly IEnumerable<string> enumerable;
        public EnumerableObservable(IEnumerable<string> enumerable)
        {
            this.enumerable = enumerable;
            this.cancellationSource = new CancellationTokenSource();
            this.cancellationToken = cancellationSource.Token;
            this.workerTask = Task.Factory.StartNew(() =>
                {
                    foreach (var value in this.enumerable)
                    {
                        //if task cancellation triggers, raise the proper exception
                        //to stop task execution
                        cancellationToken.ThrowIfCancellationRequested();

                        foreach (var observer in observerList)
                            observer.OnNext(value);
                    }
                }, this.cancellationToken);
        }

        //the cancellation token source for starting stopping
        //inner observable working thread
        private readonly CancellationTokenSource cancellationSource;
        //the cancellation flag
        private readonly CancellationToken cancellationToken;
        //the running task that runs the inner running thread
        private readonly Task workerTask;
        //the observer list
        private readonly List<IObserver<string>> observerList = new List<IObserver<string>>();
        public IDisposable Subscribe(IObserver<string> observer)
        {
            observerList.Add(observer);

            //subscription lifecycle missing
            //for readability purpose
            return null;
        }

        public void Dispose()
        {
            //trigger task cancellation
            //and wait for acknoledge
            if (!cancellationSource.IsCancellationRequested)
            {
                cancellationSource.Cancel();
                while (!workerTask.IsCanceled)
                    Thread.Sleep(100);
            }

            cancellationSource.Dispose();
            workerTask.Dispose();

            foreach (var observer in observerList)
                observer.OnCompleted();
        }
    }

    public sealed class ConsoleStringObserver : IObserver<string>
    {
        public void OnCompleted()
        {
            Console.WriteLine("-> END");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("-> {0}", error.Message);
        }

        public void OnNext(string value)
        {
            Console.WriteLine("-> {0}", value);
        }
    }
}
