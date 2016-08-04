using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _01_IObserver
{
    class Program
    {
        static void Main(string[] args)
        {
            //creates a new console input consumer
            var consumer = new ConsoleTextConsumer();

            while (true)
            {
                Console.WriteLine("Write some text and press ENTER to send a message\r\nPress ENTER to exit");
                //read console input
                var input = Console.ReadLine();

                //check for empty messate to exit
                if (string.IsNullOrEmpty(input))
                {
                    //job completed
                    consumer.OnCompleted();

                    Console.WriteLine("Task completed. Any further message will generate an error");
                }
                else
                {
                    //route the message to the consumer
                    consumer.OnNext(input);
                }
            }
        }
    }

    public class ConsoleTextConsumer : IObserver<string>
    {
        private bool finished = false;
        public void OnCompleted()
        {
            if (finished)
            {
                OnError(new Exception("This consumer already finished it's lifecycle"));
                return;
            }

            finished = true;
            Console.WriteLine("<- END");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("<- ERROR");
            Console.WriteLine("<- {0}", error.Message);
        }

        public void OnNext(string value)
        {
            if (finished)
            {
                OnError(new Exception("This consumer finished it's lifecycle"));
                return;
            }

            //shows the received message
            Console.WriteLine("-> {0}", value);
            //do something

            //ack the caller
            Console.WriteLine("<- OK");
        }
    }
}
