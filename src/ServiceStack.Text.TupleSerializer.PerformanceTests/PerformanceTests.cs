﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack.Text.TupleSerializer.Api;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.PerformanceTests
{
    public class PerformanceTests
    {
        private const int TEST_ITERATIONS = 20000000;

        private readonly string[] letters =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v",
            "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

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

        private readonly Dictionary<string, ITupleSerializer<Tuple<string, string>>> serializersToCompare = new Dictionary<string, ITupleSerializer<Tuple<string, string>>>
        {
            {"ConcurrentDictionary Cache", new TupleSerializer<Tuple<string, string>>()},
            {"ReadWriterLockSlimDictionary Cache", new TupleSerializer<Tuple<string, string>>(new ReadWriterLockSlimDictionaryCache<Tuple<string, string>, string>(), new ReadWriterLockSlimDictionaryCache<string, Tuple<string, string>>())},
            {"SynchronizedDictionary Cache", new TupleSerializer<Tuple<string, string>>(new SynchronizedDictionaryCache<Tuple<string, string>, string>(), new SynchronizedDictionaryCache<string, Tuple<string, string>>())},
            {"No Cache", new TupleSerializer<Tuple<string, string>>(new PassThroughCache<Tuple<string, string>, string>(), new PassThroughCache<string, Tuple<string, string>>())},
        };

        [Fact]
        public void Serialization()
        {
            var tupleBag = CreateTuples();

            foreach (var tupleSerializer in serializersToCompare)
            {
                PerformanceTestHarness(tupleBag, tupleSerializer.Key, tupleSerializer.Value.GetStringValue);
            }
        }

        [Fact]
        public void Deserialization()
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

            f(values[0]);

            using (new Stopper(serializerType))
            {
                Parallel.For(0, TEST_ITERATIONS, new ParallelOptions { MaxDegreeOfParallelism = 5 }, i =>
                {
                    var r = rnd.Next(values.Count);
                    f(values[r]);
                });
            }
        }

        public class Stopper : IDisposable
        {
            private readonly string _systemUnderTest;
            private readonly Stopwatch _stopWatch = new Stopwatch();

            public Stopper(string systemUnderTest)
            {
                _systemUnderTest = systemUnderTest;
                _stopWatch.Start();
            }

            public void Dispose()
            {
                _stopWatch.Stop();

                var ts = _stopWatch.Elapsed;
                var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine(_systemUnderTest + " " + elapsedTime);
            }
        }
    }
}
