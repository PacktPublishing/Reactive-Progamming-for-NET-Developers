using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Chapter5
{
    class Program
    {
        static void Main(string[] args)
        {
            //a finite sequence of 5 values
            var source = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(5)
                .Select(x => DateTime.Now)
                .Select(x =>
                {

            //lets raise some error
            if (x.Second % 10 == 0)
                        throw new ArgumentException("Wrong milliseconds value");
                    else
                        return x;
                })
                //restart he sourcing sequence on error (max 2 times)
                .Retry(2)
                //materialize to read message metadata
                .Materialize();

            source.Subscribe(x => Console.WriteLine(x));
            Console.ReadLine();
        }
    }
}
