using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _05_Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            //the action from the FromEvent
            Action<string> fromEventAction = null;

            //setup the FromEvent sequence
            Observable.FromEvent<string>(
                //register the inner action
                innerAction => { fromEventAction = innerAction; },
                //unregister the inner action
                innerAction => { fromEventAction = null; }
                )
                .Subscribe(x => Console.WriteLine("-> {0}", x));

            while (true)
            {
                //invoke the inner action
                fromEventAction(DateTime.Now.ToString());
                Thread.Sleep(1000);
            }
        }
    }
}
