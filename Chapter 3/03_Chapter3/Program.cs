using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _03_Chapter3
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s1 = new Subject<string>();
            //var s2 = new Subject<string>();
            //var merge = s1.Merge(s2);
            //merge.Subscribe(Console.WriteLine);

            //s1.OnNext("value1"); //first subject
            //s2.OnNext("value2"); //second subject

            //var s3 = new Subject<string>();
            //var delay = s3.Delay(TimeSpan.FromSeconds(10));
            //delay.Subscribe(Console.WriteLine);

            //s3.OnNext("value1");
            //s3.OnNext("value2");

            //var invoiceSummarySubject = new Subject<double>();
            //var invoiceSummaryScanSubject = invoiceSummarySubject.Scan((last, x) => x + last);
            ////register an observer for printing total amount
            //invoiceSummaryScanSubject.Subscribe(new Action<double>(x => Console.WriteLine("Total amount: {0:C}", x)));
            ////register some invoice item total
            //invoiceSummarySubject.OnNext(1250.50); //add a notebook
            //invoiceSummarySubject.OnNext(-50.0); //discount
            //invoiceSummarySubject.OnNext(44.98); //a notebook bag

            //var samplingValueSequence = new Subject<int>();
            //var samplingTimeSequence = new Subject<object>();
            //var samplingSequence = samplingValueSequence.Sample(samplingTimeSequence);
            ////register an observer
            //samplingSequence.Subscribe(new Action<int>(x => Console.WriteLine(x)));

            ////some value
            //samplingValueSequence.OnNext(10); //ignored
            //samplingValueSequence.OnNext(20);
            ////raise a message into the sampling time sequence
            //samplingTimeSequence.OnNext(null); //last value will be outputted now
            //samplingValueSequence.OnNext(30); //ignored
            //samplingValueSequence.OnNext(40);
            ////raise a message into the sampling time sequence
            //samplingTimeSequence.OnNext(null); //last value will be outputted now

            //var s4 = new Subject<string>();
            ////a numeric sequence
            //var map = s4.Select(x => double.Parse(x));
            //map.Subscribe(x => Console.WriteLine("{0:N4}", x));
            //s4.OnNext("10.40");
            //s4.OnNext("12.55");

            //var s5 = new Subject<DateTime>();
            //var throttle = s5.Throttle(TimeSpan.FromMilliseconds(500));
            //throttle.Subscribe(x => Console.WriteLine("{0:T}", x));

            ////produce 100 messages
            //for (int i = 0; i < 100; i++)
            //    s5.OnNext(DateTime.Now);

            //var s6 = new Subject<string>();
            //var s7 = new Subject<int>();
            //var clatest = s6.CombineLatest(s7, (x, y) => new { text = x, value = y, });
            //clatest.Subscribe(x => Console.WriteLine("{0}: {1}", x.text, x.value));

            ////some message
            //s6.OnNext("Mr. Brown");
            //s7.OnNext(10);
            //s7.OnNext(20);
            //s6.OnNext("Mr. Green");
            //s6.OnNext("Mr. White");
            //s7.OnNext(30);

            //var s8 = new Subject<string>();
            //var s9 = new Subject<string>();
            //var concat = s8.Concat(s9);
            //concat.Subscribe(Console.WriteLine);

            ////some message
            //s8.OnNext("value1");
            //s8.OnNext("value2");
            //s9.OnNext("value3"); //missed
            //s9.OnNext("value4"); //missed
            //s8.OnNext("value5");
            ////close first sequence
            //s8.OnCompleted();
            ////only now messages from second sequence will start flowing
            //s9.OnNext("value6");

            //var s10 = new Subject<string>();
            //var swith = s10.StartWith("value0");
            //swith.Subscribe(Console.WriteLine);
            //s10.OnNext("value1");
            //s10.OnNext("value2");

            //var s11 = new Subject<string>();
            //var s12 = new Subject<double>();
            //var zip = s11.Zip(s12, (x, y) => new { text = x, value = y });
            //zip.Subscribe(x => Console.WriteLine("{0}: {1}", x.text, x.value));
            ////same example of combine latest
            //s11.OnNext("Mr. Brown");
            //s12.OnNext(10);
            //s12.OnNext(20);
            //s11.OnNext("Mr. Green");
            //s11.OnNext("Mr. White");
            //s12.OnNext(30);
            ////this time the output is synchronized

            //var s12 = new Subject<string>();
            //var filtered = s12.Where(x => x.Contains("e"));
            //filtered.Subscribe(Console.WriteLine);
            //s12.OnNext("Mr. Brown");
            //s12.OnNext("Mr. White");

            //var s13 = new Subject<string>();
            //var distinct = s13.Distinct();
            //distinct.Subscribe(Console.WriteLine);
            //s13.OnNext("value1");
            //s13.OnNext("value2");
            //s13.OnNext("value1");
            //s13.OnNext("value2");

            //var s12 = new Subject<string>();
            //var distinct = s12.DistinctUntilChanged();
            //distinct.Subscribe(Console.WriteLine);
            //s12.OnNext("value1"); //ok
            //s12.OnNext("value2"); //ok
            //s12.OnNext("value2"); //ignored
            //s12.OnNext("value3"); //ok
            //s12.OnNext("value4"); //ok

            //var s13 = new Subject<string>();
            //var indexed = s13.ElementAt(2);

            //indexed.Subscribe(Console.WriteLine);
            //s13.OnNext("value1"); //ignored
            //s13.OnNext("value2"); //ignored
            //s13.OnNext("value3"); //OK
            //s13.OnNext("value4"); //ignored

            //var s14 = new Subject<string>();
            //var skip = s14.Skip(2);
            //skip.Subscribe(Console.WriteLine);
            //s14.OnNext("value1"); //ignored 
            //s14.OnNext("value2"); //ignored
            //s14.OnNext("value3"); //ok
            //s14.OnNext("value4"); //ok

            //var s15 = new Subject<string>();
            //var take = s15.Take(2);
            //take.Subscribe(Console.WriteLine);
            //s15.OnNext("value1"); //ok 
            //s15.OnNext("value2"); //ok
            //s15.OnNext("value3"); //ignored
            //s15.OnNext("value4"); //ignored

            //var s16 = new Subject<double>();
            //var min = s16.Min(); //register for finding the min
            //var max = s16.Max(); //register for finding the max
            //var avg = s16.Average(); //register for finding the average
            //var sum = s16.Sum(); //register for finding the count
            //var count = s16.Count(); //register for finding the sum

            //min.Subscribe(x => Console.WriteLine("min: {0}", x));
            //max.Subscribe(x => Console.WriteLine("max: {0}", x));
            //avg.Subscribe(x => Console.WriteLine("avg: {0}", x));
            //sum.Subscribe(x => Console.WriteLine("sum: {0}", x));
            //count.Subscribe(x => Console.WriteLine("count: {0}", x));

            ////some value
            //var r = new Random(DateTime.Now.GetHashCode());
            //for (int i = 0; i < 10; i++)
            //    s16.OnNext(r.NextDouble() * 100d);

            ////now aggregation operators fill flow their message
            //s16.OnCompleted();

            //var s17 = new Subject<double>();
            //var every = s17.All(x => x > 0);
            //var some = s17.Any(x => x % 2 == 0);
            //var includes = s17.Contains(4d);

            //every.Subscribe(x => Console.WriteLine("every: {0}", x));
            //some.Subscribe(x => Console.WriteLine("some: {0}", x));
            //includes.Subscribe(x => Console.WriteLine("includes: {0}", x));

            ////some value
            //var r = new Random(DateTime.Now.GetHashCode());
            //for (int i = 0; i < 10; i++)
            //    s17.OnNext(r.NextDouble() * 100d);

            ////now operators fill flow their message
            //s17.OnCompleted();

            //var s18 = new Subject<int>();
            //var s19 = new Subject<int>();
            //var equals = s18.SequenceEqual(s19);
            //equals.Subscribe(x => Console.WriteLine("sequenceEqual: {0}", x));
            //s18.OnNext(10);
            //s18.OnNext(20);
            //s19.OnNext(10);
            //s19.OnNext(20);
            //s18.OnNext(30);
            //s19.OnNext(30);

            ////completes to flow out the sequenceEqual result message
            //s18.OnCompleted();
            //s19.OnCompleted();

            //var s20 = new Subject<string>();
            //var s21 = new Subject<string>();
            //var amb = s20.Amb(s21);
            //amb.Subscribe(Console.WriteLine);

            ////the first message will let amb operator
            ////choose the definite source sequence

            //s21.OnNext("value1");
            ////messages from the other sequences are ignored
            //s20.OnNext("value2");


            var s24 = new Subject<string>();
            var distinct = s24.DistinctUntilChanged();
            distinct.Subscribe(Console.WriteLine);
            s24.OnNext("value1"); //ok
            s24.OnNext("value2"); //ok
            s24.OnNext("value2"); //ignored
            s24.OnNext("value3"); //ok
            s24.OnNext("value4"); //ok
            s24.OnNext("value1"); //ok
            s24.OnNext("value2"); //ok
            s24.OnNext("value2"); //ignored
            s24.OnNext("value3"); //ok
            s24.OnNext("value4"); //ok
            s24.OnNext("value1"); //ok
            s24.OnNext("value2"); //ok
            s24.OnNext("value2"); //ignored
            s24.OnNext("value3"); //ok
            s24.OnNext("value4"); //ok


            Console.ReadLine();
        }
    }
}
