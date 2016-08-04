using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace _04_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            int value = 0;

            //a scheduler
            var scheduler = Scheduler.Default;

            ////multiple immediate jobs
            //scheduler.Schedule(x => { value = 14; });
            //scheduler.Schedule(x => { Console.WriteLine(value); });
            //scheduler.Schedule(x => { value = 15; });
            //scheduler.Schedule(x => { Console.WriteLine(value); });
            //scheduler.Schedule(x => { value = 16; });
            //scheduler.Schedule(x => { Console.WriteLine(value); });

            value = 14;
            scheduler.Schedule<int>(value, (_scheduler, state) =>
            {
                Console.WriteLine(state);
                return Disposable.Empty;
            });

            value = 15;
            scheduler.Schedule<int>(value, (_scheduler, state) =>
            {
                Console.WriteLine(state);
                return Disposable.Empty;
            });

            value = 16;
            scheduler.Schedule<int>(value, (_scheduler, state) =>
            {
                Console.WriteLine(state);
                return Disposable.Empty;
            });

            Console.ReadLine();
        }
    }
}
