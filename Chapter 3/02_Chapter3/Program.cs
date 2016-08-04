using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace _02_Chapter3
{
    class Program
    {
        static void Main(string[] args)
        {
            var receiverSubject = new Subject<string>();
            //the final subject implementation
            receiverSubject.Subscribe(x => Console.WriteLine("s1=>{0}", x));

            //the source of al messages
            var senderSubject = new Subject<string>();
            //no observers here

            //the router made with the Observer part of
            //the receiverSubject and the Observable part
            //of the senderSubject
            var routerSubject = Subject.Create(receiverSubject, senderSubject);
            //another observer for testing purposes
            routerSubject.Subscribe(x => Console.WriteLine("s3=>{0}", x));

            senderSubject.OnNext("value1");
            senderSubject.OnNext("value2");

            Console.ReadLine();
        }
    }
}
