using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _06_CorrelatedEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var publisher = new NewFileSavedMessagePublisher(@"c:\temp"))
            //creates a new correlator by specifying the correlation key
            //extraction function made with a Regular expression that
            //extract a file ID similar to FILEID0001
            using (var correlator = new FileNameMessageCorrelator(ExtractCorrelationKey))
            {
                //subscribe the correlator to publisher messages
                publisher.Subscribe(correlator);

                //subscribe the console subscriber to the correlator
                //instead that directly to the publisher
                correlator.Subscribe(new NewFileSavedMessageSubscriber());

                //wait for user RETURN
                Console.ReadLine();
            }
        }

        private static string ExtractCorrelationKey(string arg)
        {
            var match = Regex.Match(arg, "(FILEID\\d{4})");
            if (match.Success)
                return match.Captures[0].Value;
            else
                return null;
        }
    }

    public sealed class NewFileSavedMessagePublisher : IObservable<string>, IDisposable
    {
        private readonly FileSystemWatcher watcher;
        public NewFileSavedMessagePublisher(string path)
        {
            //creates a new file system event router
            this.watcher = new FileSystemWatcher(@"C:\Users\USER\Desktop\4954\ss");
            //register for handling File Created event
            this.watcher.Created += watcher_Created;
            //enable event routing
            this.watcher.EnableRaisingEvents = true;
        }

        //signal all observers a new file arrived
        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            foreach (var observer in observerList)
                observer.OnNext(e.FullPath);
        }

        //the subscriber list
        private readonly List<IObserver<string>> observerList = new List<IObserver<string>>();

        public IDisposable Subscribe(IObserver<string> observer)
        {
            //register the new observer
            observerList.Add(observer);

            return null;
        }

        public void Dispose()
        {
            //disable file system event routing
            this.watcher.EnableRaisingEvents = false;
            //deregister from watcher event handler
            this.watcher.Created -= watcher_Created;
            //dispose the watcher
            this.watcher.Dispose();

            //signal all observers that job is done
            foreach (var observer in observerList)
                observer.OnCompleted();
        }
    }

    /// <summary>
    /// A tremendously basic implementation
    /// </summary>
    public sealed class NewFileSavedMessageSubscriber : IObserver<string>
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

    public sealed class FileNameMessageCorrelator : IObservable<string>, IObserver<string>, IDisposable
    {
        private readonly Func<string, string> correlationKeyExtractor;
        public FileNameMessageCorrelator(Func<string, string> correlationKeyExtractor)
        {
            this.correlationKeyExtractor = correlationKeyExtractor;
        }

        //the observer collection
        private readonly List<IObserver<string>> observerList = new List<IObserver<string>>();
        public IDisposable Subscribe(IObserver<string> observer)
        {
            this.observerList.Add(observer);
            return null;
        }

        private bool hasCompleted = false;
        public void OnCompleted()
        {
            hasCompleted = true;
            foreach (var observer in observerList)
                observer.OnCompleted();
        }

        //routes error messages until not completed
        public void OnError(Exception error)
        {
            if (!hasCompleted)
                foreach (var observer in observerList)
                    observer.OnError(error);
        }

//the container of correlations able to contain
//multiple strings per each key
private readonly NameValueCollection correlations = new NameValueCollection();

//routes valid messages until not completed
public void OnNext(string value)
{
    if (hasCompleted) return;

    //check if subscriber has completed
    Console.WriteLine("Parsing message: {0}", value);

    //try extracting the correlation ID
    var correlationID = correlationKeyExtractor(value);

    //check if the correlation is available
    if (correlationID == null) return;

    //append the new file name to the correlation state
    correlations.Add(correlationID, value);

    //in this example we will consider always
    //correlations of two items
    if (correlations.GetValues(correlationID).Count() == 2)
    {
        //once the correlation is complete
        //read the two files and push the
        //two contents altogether to the
        //observers

        var fileData = correlations.GetValues(correlationID)
            //route messages to the ReadAllText method
            .Select(File.ReadAllText)
            //materialize the query
            .ToArray();

        var newValue = string.Join("|", fileData);

        foreach (var observer in observerList)
            observer.OnNext(newValue);

        correlations.Remove(correlationID);
    }
}

        public void Dispose()
        {
            OnCompleted();
        }
    }
}
