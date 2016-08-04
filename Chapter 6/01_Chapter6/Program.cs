using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01_Chapter6
{
    class Program
    {
        public static event EventHandler userIsTiredEvent;
        static void Main(string[] args)
        {
            Console.WriteLine($"{0} {1}");

            //raise the event in 5 seconds
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);

                //check event is handled
                if (userIsTiredEvent != null)
                    userIsTiredEvent("Program.Main", new EventArgs());
            });

            //classic event handler registration
            userIsTiredEvent += EventHandler1;

            //reactive registration
            var eventSequence = Observable.FromEventPattern(typeof(Program), "userIsTiredEvent");

            //some output
            eventSequence.Materialize().Subscribe(x => Console.WriteLine("From Rx: {0}", x));

            Console.ReadLine();
        }

        static void EventHandler1(object o, EventArgs e)
        {
            Console.WriteLine("Handling for object {0}", o);
        }
    }
}
