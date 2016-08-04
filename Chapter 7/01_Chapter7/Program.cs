using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            //1000 items to source from
            var items = Enumerable.Range(0, 1000)
                .Select(x =>
                {
                    //raise an exception on item #400
                    //within VS the debugger will stop the execution as the exception bubbles
                    //simply press F5 again to continue bubble the exception to Rx sequence
                    if (x == 400)
                        throw new ArgumentException("The item #400 has been sourcing");
                    

                    return x;
                });

            //invoke our custom operator
            var sequence = items.AsObservable();

            //output value and metadata
            sequence.Materialize().Subscribe(x => Console.WriteLine("-> {0}", x));

            Console.ReadLine();
        }
    }

    public static class RxOperators
    {
        public static IObservable<T> AsObservable<T>(this IEnumerable<T> source)
        {
            return Observable.Create<T>(observer =>
            {
                foreach (var item in source)
                    try
                    {
                        observer.OnNext(item);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        break;
                    }

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
    }
}
