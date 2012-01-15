using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Paralect.Machine.Utilities;

namespace Paralect.Machine.Tests.Areas.Performance.Fixtures
{
    [TestFixture]
    public class DictionaryTest
    {
        private const int COUNT         = 100000;
        private const int NUMBER        = 10000;

        [Ignore]
        public void GuidTest()
        {
            Do(() => Guid.NewGuid());
        }

        [Ignore]
        public void StringTest()
        {
            Do(() => Guid.NewGuid().ToString());
        }

        [Ignore]
        public void LongStringTest()
        {
            Do(() =>
            {
                var part = Guid.NewGuid().ToString();
                return String.Format("{0}{1}{2}{4}{5}{6}", part, part, part, part, part, part, part); // yeah... i know
            });
        }

        [Ignore]
        public void BinaryTest()
        {
            Do(() => Guid.NewGuid().ToByteArray(), 
               () => new Dictionary<byte[], int>(COUNT, new ByteArrayComparer()));
        }

        private void Do<TKey>(Func<TKey> keyFactory)
        {
            Do(keyFactory, () => new Dictionary<TKey, int>(COUNT));
        }

        private void Do<TKey>(Func<TKey> keyFactory, Func<Dictionary<TKey, int>> dictionaryFactory)
        {
            Console.WriteLine("As {0}.", typeof(TKey));

            Stopwatch watch = Stopwatch.StartNew();
            Dictionary<TKey, int> keyValueStore = dictionaryFactory();//new Dictionary<TKey, int>(COUNT);
            List<TKey> listStore = new List<TKey>(COUNT);

            for (int i = 0; i < COUNT; i++)
            {
                TKey guid = keyFactory();
                keyValueStore.Add(guid, 1);

                if (i < NUMBER)
                    listStore.Add(guid);
            }
            watch.Stop();

            Console.WriteLine("\tAllocation speed: {0}", watch.ElapsedMilliseconds);

            watch = Stopwatch.StartNew();

            int sum = 0;
            for (int i = 0; i < NUMBER; i++)
            {
                var guid = listStore[i];
                sum += keyValueStore[guid];
            }

            watch.Stop();

            Console.WriteLine("\tTotal lookup speed: {1}, Number of lookups: {2}", typeof(TKey), watch.ElapsedMilliseconds, sum);
        }


        /// <summary>
        /// TODO: Run under memory profiler to see pages fragmentation.
        /// </summary>
        [Ignore]
        public void Simple()
        {
            const int COUNT = 1000000;
            Dictionary<Guid, int> guids = new Dictionary<Guid, int>(COUNT);
            Dictionary<string, int> strings = new Dictionary<string, int>(COUNT, StringComparer.Ordinal);

            Dictionary<byte[], int> array = new Dictionary<byte[], int>(COUNT);

            List<String> strs = new List<string>(COUNT);
            List<Guid> gds = new List<Guid>(COUNT);

            for (int i = 0; i < COUNT; i++)
            {
                Guid guid = Guid.NewGuid();
                guids.Add(guid, 1);

                var st = guid.ToString();
                string hugehugeString = String.Format("{0}{1}{2}{3}{4}{5}", st, st, st, st, st, st);

                strings.Add(hugehugeString, 1);

                if (i < 1000000)
                {
                    strs.Add(hugehugeString);
                    gds.Add(guid);
                }
            }

            Console.WriteLine("Done.");
            Console.WriteLine("String size {0}", strs[0].Length);

            Stopwatch watch = Stopwatch.StartNew();
            int z = 0;
            for (int i = 0; i < 1000000; i ++)
            {
                var st = strs[i];

                z += strings[st];
            }

            watch.Stop();

            Console.WriteLine("As strings: " + watch.ElapsedMilliseconds+ "; " + z);

            watch = Stopwatch.StartNew();
            int z1 = 0;
            for (int i = 0; i < 1000000; i++)
            {
                var st = gds[i];

                z1 += guids[st];
            }

            watch.Stop();
            
            Console.WriteLine("As guids: " + watch.ElapsedMilliseconds + "; " + z1);
        }
    }
}
