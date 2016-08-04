using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _07_StreamEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            //listen to port 8081
            using (var observable = new TcpListenerStringObservable(8081))
            using (var observer = observable.Subscribe(new ConsoleStringObserver()))
            {
                //wait until use press RETURN
                Console.WriteLine("Press RETURN to EXIT");
                Console.ReadLine();
            }
        }
    }

    public sealed class TcpListenerStringObservable : IObservable<string>, IDisposable
    {
        private readonly TcpListener listener;
        public TcpListenerStringObservable(int port, int backlogSize = 64)
        {
            //creates a new tcp listener on given port
            //with given backlog size
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start(backlogSize);

            //start listening asynchronously
            listener.AcceptTcpClientAsync().ContinueWith(OnTcpClientConnected);
        }

        private void OnTcpClientConnected(Task<TcpClient> clientTask)
        {
            //if the task has not encountered errors
            if (clientTask.IsCompleted)
                Task.Factory.StartNew(() =>
                    {
                        using (var tcpClient = clientTask.Result)
                        using (var stream = tcpClient.GetStream())
                        using (var reader = new StreamReader(stream))
                            while (tcpClient.Connected)
                            {
                                //read the message
                                var line = reader.ReadLine();

                                //stop listening if nothing available
                                if (string.IsNullOrEmpty(line))
                                    break;
                                else
                                {
                                    //construct observer message adding client's remote endpoint address and port
                                    var msg = string.Format("{0}: {1}", tcpClient.Client.RemoteEndPoint, line);

                                    //route messages
                                    foreach (var observer in observerList)
                                        observer.OnNext(msg);
                                }
                            }
                    }, TaskCreationOptions.PreferFairness);

            //starts another client listener
            listener.AcceptTcpClientAsync().ContinueWith(OnTcpClientConnected);
        }

        private readonly List<IObserver<string>> observerList = new List<IObserver<string>>();
        public IDisposable Subscribe(IObserver<string> observer)
        {
            observerList.Add(observer);

            //subscription lifecycle missing
            //for readability purpose
            return null;
        }

        public void Dispose()
        {
            //stop listener
            listener.Stop();
        }
    }

    public sealed class ConsoleStringObserver : IObserver<string>
    {
        public void OnCompleted()
        {
            Console.WriteLine("-> END");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("-> {0}", error.Message);
        }

        public void OnNext(string value)
        {
            Console.WriteLine("-> {0}", value);
        }
    }
}
