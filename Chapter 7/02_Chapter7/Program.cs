using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            ////convert a TcpListener into an observable sequence on port 23 (telnet)
            //var tcpClientsSequence = TcpListener.Create(23)
            //    .AcceptObservableClient();

            ////subscribe to newly remote clients
            //var observer = tcpClientsSequence.Subscribe(client =>
            //{
            //    //remote endpoint (IP:PORT)
            //    var endpoint = client.Client.RemoteEndPoint as IPEndPoint;
            //    Console.Write("{0} -> ", endpoint);
            //    //get the remote stream
            //    using (var stream = client.GetStream())
            //        while (true)
            //        {
            //            //read bytes until available
            //            var b = stream.ReadByte();
            //            if (b < 0)
            //                break;
            //            else
            //                Console.Write((char)b);
            //        }
            //    Console.WriteLine();
            //    Console.WriteLine("{0} -> END", endpoint);
            //    Console.WriteLine();
            //});




            //convert a TcpListener into an observable sequence on port 23 (telnet)
            var tcpClientsSequence = TcpListener.Create(23)
                .AcceptObservableClient()
                .AsNetworkByteSource();

            //map the source message into another with a byte buffer of a single byte
            var bufferUntileCRLFSequence = tcpClientsSequence
                .Select(x => new { x.Key, buffer = new[] { x.Value }.AsEnumerable() })
                //group by client session IPEndPoint (IP/Port)
                .GroupBy(x => x.Key);

            //a crlf byte buffer
            var crlf = new byte[] { 0x000d, 0x000a };

            //subscribe to all nested sequence groups per remote endpoint
            bufferUntileCRLFSequence.Subscribe(endpoint =>
            {
                var clientSequence = endpoint
                    //apply an accumulator function to obtain the byte buffer per client
                    //the function will check if the buffer terminates with the CRLF then in the case will create a new buffer otherwise it will concat the previous buffer with the new byte
                    .Scan((last, i) => new { last.Key, buffer = last.buffer.Skip(last.buffer.Count() - 2).SequenceEqual(crlf) ? i.buffer : last.buffer.Concat(i.buffer) })
                    //wait the CR+LF message to read per row
                    .Where(x => x.buffer.Skip(x.buffer.Count() - 2).SequenceEqual(crlf));

                //subscribe to the client sequence
                clientSequence.Subscribe(row => 
                    Console.WriteLine("{0} -> {1}", row.Key, Encoding.ASCII.GetString(row.buffer.ToArray())));
            });

            Console.ReadLine();
        }
    }

    public static class NetworkObservables
    {
        //the extension method must be put in a static class
        public static IObservable<TcpClient> AcceptObservableClient(this TcpListener listener)
        {
            //start listening with a 4 clients buffer backlog
            listener.Start(4);

            return Observable.Create<TcpClient>(observer =>
            {
                while (true)
                {
                    //accept newly clients from the listener
                    var client = listener.AcceptTcpClient();
                    //route the client to the observer
                    //into an asynchronous task to let multiple clients connect altogether  
                    Task.Factory.StartNew(() => observer.OnNext(client), TaskCreationOptions.LongRunning);
                }

                //mandatory to comply with the .Create action signature
                return Disposable.Empty;
            });
        }

        public static IObservable<KeyValuePair<IPEndPoint, byte>> AsNetworkByteSource(this IObservable<TcpClient> source)
        {
            return Observable.Create<KeyValuePair<IPEndPoint, byte>>(observer =>
            {
                using (var innerObserver = source.Subscribe(client =>
                    {
                        using (var stream = client.GetStream())
                            while (true)
                            {
                                var b = stream.ReadByte();
                                if (b < 0)
                                    break;
                                else
                                    observer.OnNext(new KeyValuePair<IPEndPoint, byte>(client.Client.RemoteEndPoint as IPEndPoint, (byte)b));
                            }
                    }))
                {
                    //dispose the innerObserver when completes
                }

                observer.OnCompleted();

                //mandatory to comply with the .Create action signature
                return Disposable.Empty;
            });
        }
    }
}
