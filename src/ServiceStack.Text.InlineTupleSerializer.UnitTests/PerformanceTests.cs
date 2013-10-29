using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class PerformanceTests
    {
        private const int TEST_ITERATIONS = 1000000;

        private readonly string[] letters =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v",
            "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

        private string ipsum =
            @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer fringilla malesuada imperdiet. Nulla aliquam accumsan dui vel euismod. Sed laoreet congue augue, accumsan iaculis nibh sollicitudin sed. Donec commodo faucibus ligula, eu dignissim arcu dignissim a. Donec tellus justo, rutrum quis pulvinar vitae, vehicula quis urna. Mauris accumsan aliquet hendrerit. Cras commodo auctor nunc, a pharetra ante accumsan at. Aliquam semper elit nec orci auctor, eget facilisis erat feugiat.
In a eros ut nulla rutrum varius. Nunc porttitor non nisl sed ultricies. Etiam quis nulla id libero elementum imperdiet pulvinar quis urna. Sed eu sollicitudin quam. Nulla non aliquet leo. Quisque viverra dapibus ligula at pretium. Proin posuere sed lectus ut ultricies. Cras pharetra sapien non quam dapibus, sodales euismod ipsum molestie. Phasellus libero elit, mattis ac viverra vitae, rhoncus non dolor.
Suspendisse potenti. Interdum et malesuada fames ac ante ipsum primis in faucibus. Sed sollicitudin mi eu sodales sagittis. Etiam commodo nisi nec dolor feugiat tincidunt. Cras porta, nisl quis porttitor scelerisque, purus urna bibendum dui, ullamcorper auctor velit tortor eget dui. Vivamus rutrum pharetra dolor, at eleifend diam commodo vitae. Fusce vulputate tellus nisi, at tempus risus porta et. Nam arcu enim, semper in nibh nec, feugiat cursus enim. Morbi diam elit, rutrum condimentum massa eu, sagittis porta urna. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Maecenas volutpat massa in imperdiet vulputate. Aliquam tincidunt pharetra odio non feugiat.
Suspendisse pulvinar porttitor odio. Phasellus justo magna, dignissim vitae lorem ut, lobortis elementum enim. Morbi sed urna quis nisi condimentum pretium. Nam porttitor, dui sodales euismod vestibulum, enim orci lobortis justo, at fringilla sem dolor eu lectus. Nam quis mollis tellus, et rhoncus urna. Aenean in enim enim. Curabitur quis fermentum nisi. Vivamus mollis venenatis auctor. Praesent molestie pellentesque mi, id scelerisque arcu egestas sit amet. Nam tempor auctor iaculis. Cras ac pulvinar lacus. Suspendisse potenti. Duis nulla magna, dignissim id fringilla et, fringilla vel sapien. Quisque eu urna id erat fermentum fringilla vel sit amet nibh.
Donec ultricies ante vitae nisi cursus, quis auctor lacus mollis. Aenean laoreet, purus at euismod lobortis, quam nibh laoreet ligula, sit amet pharetra lorem nibh et libero. Mauris porta orci et tincidunt bibendum. Fusce sem purus, dapibus a ligula vel, porttitor bibendum turpis. Aenean tristique fermentum sodales. Integer eu risus sit amet est consectetur fringilla eu in ipsum. Sed vel quam ullamcorper, commodo urna eu, fringilla risus. Suspendisse urna est, faucibus vitae sollicitudin non, molestie gravida massa. Morbi ac neque at felis tempor posuere.";

        public List<Tuple<string, string>> CreateTuples()
        {
            var pairs = new List<Tuple<string, string>>();
            for (var i = 0; i < letters.Length - 1; i++)
            {
                for (var j = i + 1; j < letters.Length; j++)
                {
                    pairs.Add(Tuple.Create(letters[i], letters[j]));
                }
            }
            return pairs;
        }

        public List<Tuple<string, string>> CreateTuples2()
        {
            var words = ipsum.Split(null);
            var pairs = new List<Tuple<string, string>>();
            for (var i = 0; i < words.Length - 1; i++)
            {
                for (var j = i + 1; j < words.Length; j++)
                {
                    pairs.Add(Tuple.Create(words[i], words[j]));
                }
            }
            return pairs;
        }

        public List<string> CreateStringPairs()
        {
            var pairs = new List<string>();
            for (var i = 0; i < letters.Length - 1; i++)
            {
                for (var j = i + 1; j < letters.Length; j++)
                {
                    pairs.Add(letters[i] + "-" + letters[j]);
                }
            }
            return pairs;
        }

        public List<string> CreateStringPairs2()
        {
            var words = ipsum.Split(null);
            var pairs = new List<string>();
            for (var i = 0; i < words.Length - 1; i++)
            {
                for (var j = i + 1; j < words.Length; j++)
                {
                    pairs.Add(words[i] + "-" + words[j]);
                }
            }
            return pairs;
        }

        public class Stopper : IDisposable
        {
            private readonly string _systemUnderTest;
            readonly Stopwatch stopWatch = new Stopwatch();

            public Stopper(string systemUnderTest)
            {
                _systemUnderTest = systemUnderTest;
                stopWatch.Start();
            }

            public void Dispose()
            {
                stopWatch.Stop();

                var ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine(_systemUnderTest + " " + elapsedTime);
            }
        }

        private readonly Dictionary<string, ITupleSerializer<Tuple<string, string>>> serializersToCompare = new Dictionary<string, ITupleSerializer<Tuple<string, string>>>
        {
            {"ConcurrentDictionary Cache", new TupleSerializationHelpers<Tuple<string, string>>()},
            {"ReadWriterLockSlimDictionary Cache", new TupleSerializationHelpers<Tuple<string, string>>(new ReadWriterLockSlimDictionaryCache<Tuple<string, string>, string>(), new ReadWriterLockSlimDictionaryCache<string, Tuple<string, string>>())},
            {"SynchronizedDictionary Cache", new TupleSerializationHelpers<Tuple<string, string>>(new SynchronizedDictionaryCache<Tuple<string, string>, string>(), new SynchronizedDictionaryCache<string, Tuple<string, string>>())},
            {"No Cache", new TupleSerializationHelpers<Tuple<string, string>>(new PassThroughCache<Tuple<string, string>, string>(), new PassThroughCache<string, Tuple<string, string>>())},
        };

        [TestMethod]
        public void Serialization()
        {
            var rnd = new Random();
            var tupleBag = CreateTuples2();
            var serializer = new TupleSerializationHelpers<Tuple<string, string>>();

            using (var stopper = new Stopper("no cache"))
            {
                for (int i = 0; i < TEST_ITERATIONS; i++)
                {
                    int r = rnd.Next(tupleBag.Count);
                    var str = serializer.SerializeTuple(tupleBag[r]);
                }
            }

            using (var stopper = new Stopper("cache"))
            {
                for (int i = 0; i < TEST_ITERATIONS; i++)
                {
                    int r = rnd.Next(tupleBag.Count);
                    var str = serializer.GetStringValue(tupleBag[r]);
                }
            }
        }

        [TestMethod]
        public void Deserialization()
        {
            var rnd = new Random();
            var stringBag = CreateStringPairs2();
            var serializer = new TupleSerializationHelpers<Tuple<string, string>>();

            using (var stopper = new Stopper("no cache"))
            {
                for (int i = 0; i < TEST_ITERATIONS; i++)
                {
                    int r = rnd.Next(stringBag.Count);
                    var str = serializer.DeserializeTuple(stringBag[r]);
                }
            }

            using (var stopper = new Stopper("cache"))
            {
                for (int i = 0; i < TEST_ITERATIONS; i++)
                {
                    int r = rnd.Next(stringBag.Count);
                    var str = serializer.GetTupleFrom(stringBag[r]);
                }
            }
        }

        [TestMethod]
        public void Serialization2()
        {
            var tupleBag = CreateTuples();

            foreach (var tupleSerializer in serializersToCompare)
            {
                PerformanceTestHarness(tupleBag, tupleSerializer.Key, tupleSerializer.Value.GetStringValue);
            }
        }

        [TestMethod]
        public void Deserialization2()
        {
            var stringBag = CreateStringPairs();

            foreach (var tupleSerializer in serializersToCompare)
            {
                PerformanceTestHarness(stringBag, tupleSerializer.Key, tupleSerializer.Value.GetTupleFrom);
            }
        }

        private void PerformanceTestHarness<TInputType, TOutputType>(List<TInputType> values, string serializerType,
            Func<TInputType, TOutputType> f)
        {
            var rnd = new Random();

            using (new Stopper(serializerType))
            {
                for (int i = 0; i < TEST_ITERATIONS; i++)
                {
                    int r = rnd.Next(values.Count);
                    TOutputType value = f(values[r]);
                }
            }
        }
    }
}
