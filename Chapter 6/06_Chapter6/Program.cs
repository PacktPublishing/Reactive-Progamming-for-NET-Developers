using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Chapter6
{
    class Program
    {
        //a static event
        public static event Action MyStaticEvent;
        static void Main(string[] args)
        {
            //event sequence
            var sequence = Observable.FromEvent(
                //register the inner action as handler of the static event
                x => MyStaticEvent += x,
                //unregister the inner action from the static event
                x => MyStaticEvent -= x);

            //observer
            sequence.Subscribe(unit => Console.WriteLine(unit));

            //manually raise the event
            MyStaticEvent();

            Console.ReadLine();
        }
    }
}
