using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            var buffer = Enumerable.Range(0, 1000)
                //split enumerable into multiple buffers
                .Buffer(100);

            //enumerate from the first enumerable
            //in case of exception continue enumerating
            //from the second enumerable
            //similarly there is the OnErrorResumeNext operator
            var catched = Enumerable.Range(0, 1000).Catch(Enumerable.Range(0, 1000));

            //returns a list from a single instance
            var returned = EnumerableEx.Return(10);

            //retry if an error occurs
            var retry = Enumerable.Range(0, 1000).Retry(10);

            //creates an enumerable from a yielder
            var enumerable = EnumerableEx.Create<int>(async yielder =>
            {
                for (int i = 0; i < 1000; i++)
                    await yielder.Return(i);
            });

            //start yelding values
            var enumerableValues = enumerable.ToArray();

            //opposite of Any
            enumerable.IsEmpty();

            //create a finite buffer of values
            //materializing values only on usage
            //instead ToArray/ToList always materialize
            //act as a cache of elements from the source enumerable
            var memoized = enumerable.Memoize();

            //cause memoize to materialize the internal collection
            var firstValue = memoized.FirstOrDefault();

//accumulator function
var runningTotal = enumerable.Scan((old, x) => old + x);

        }
    }
}
