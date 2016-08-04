using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_SourcingEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            //the observable collection
            var collection = new ObservableCollection<string>();

            using (var observable = new NotifiableCollectionObservable(collection))
            using (var observer = observable.Subscribe(new ConsoleStringObserver()))
            {

                Console.WriteLine("ciao"); collection.Add("ciao");
                Console.WriteLine("hahahaha"); 

                collection.Insert(0, "new first line");
                collection.RemoveAt(0);

                Console.WriteLine("Press RETURN to EXIT");
                Console.ReadLine();
            }
        }

        public sealed class NotifiableCollectionObservable : IObservable<string>, IDisposable
        {
            private readonly ObservableCollection<string> collection;
            public NotifiableCollectionObservable(ObservableCollection<string> collection)
            {
                this.collection = collection;
                this.collection.CollectionChanged += collection_CollectionChanged;
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
                this.collection.CollectionChanged -= collection_CollectionChanged;

                foreach (var observer in observerList)
                    observer.OnCompleted();
            }
        }

    }

    public sealed class NewFileSavedMessagePublisher : IObservable<string>, IDisposable
    {
        private readonly FileSystemWatcher watcher;
        public NewFileSavedMessagePublisher(string path)
        {
            //creates a new file system event router
            this.watcher = new FileSystemWatcher(path);
            //register for handling File Created event
            this.watcher.Created += OnFileCreated;
            //enable event routing
            this.watcher.EnableRaisingEvents = true;
        }

        //signal all observers a new file arrived
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            foreach (var observer in subscriberList)
                observer.OnNext(e.FullPath);
        }

        //the subscriber list
        private readonly List<IObserver<string>> subscriberList = new List<IObserver<string>>();

        public IDisposable Subscribe(IObserver<string> observer)
        {
            //register the new observer
            subscriberList.Add(observer);

            return null;
        }

        public void Dispose()
        {
            //disable file system event routing
            this.watcher.EnableRaisingEvents = false;
            //deregister from watcher event handler
            this.watcher.Created -= OnFileCreated;
            //dispose the watcher
            this.watcher.Dispose();

            //signal all observers that job is done
            foreach (var observer in subscriberList)
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
}

