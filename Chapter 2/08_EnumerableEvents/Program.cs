using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_EnumerableEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            //the observable collection
            var collection = new ObservableCollection<string>();

            //register a handler to catch collection changes
            //collection.CollectionChanged += OnCollectionChanged; //disabled

            using (var observable = new NotifiableCollectionObservable(collection))
            using (var observer = observable.Subscribe(new ConsoleStringObserver()))
            {
                collection.Add("ciao");
                collection.Add("hahahah");

                collection.Insert(0, "new first line");
                collection.RemoveAt(0);

                Console.WriteLine("Press RETURN to EXIT");
                Console.ReadLine();
            }
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<string>;

            if (e.NewStartingIndex >= 0) //adding new items
                Console.WriteLine("-> {0} {1}", e.Action, collection[e.NewStartingIndex]);
            else //removing items
                Console.WriteLine("-> {0} at {1}", e.Action, e.OldStartingIndex);
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

        void collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var newValue = collection[e.NewStartingIndex];

                foreach (var observer in observerList)
                    observer.OnNext(newValue);
            }
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
