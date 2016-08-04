using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            var values2 = Observable.Range(0, 100).Where(x => x % 2 == 0);
            var values3 = Observable.Range(0, 100).Where(x => x % 3 == 0);
            var values5 = Observable.Range(0, 100).Where(x => x % 5 == 0);

            //flatten sourcing sequences into a new sequence
            //based on the sourcing message index
            var zip = values2.Zip(values3, values5, (a, b, c) => new { a, b, c });

            Console.WriteLine("Zip:");
            zip.Subscribe(x => Console.WriteLine(x));

            //create a pattern by grouping messages based on their index
            var pattern = values2.And(values3).And(values5)
                //then produce a single output
                .Then((a, b, c) => new { a, b, c });

            //creates a sequence from the pattern
            var then = Observable.When(pattern);

            Console.WriteLine("Then:");
            then.Subscribe(x => Console.WriteLine(x));

            //multiple patterns
            var values7 = Observable.Range(0, 100).Where(x => x % 7 == 0);
            var values9 = Observable.Range(0, 100).Where(x => x % 9 == 0);
            var values11 = Observable.Range(0, 100).Where(x => x % 11 == 0);

            var pattern79 = values7.And(values9).And(values11).Then((a, b, c) => new { a, b, c, });

            //flatten multiple sourcing pattern into a new sequence
            var then79 = Observable.When(pattern, pattern79);

            //the message order will follow the sourcing patterns message index
            Console.WriteLine("Then79:");
            then79.Subscribe(x => Console.WriteLine(x));

            Console.ReadLine();
        }
    }

    public static class ZipExtensions
    {

    }
}
