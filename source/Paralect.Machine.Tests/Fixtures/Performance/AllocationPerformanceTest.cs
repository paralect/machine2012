using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Paralect.Machine.Tests.Fixtures.Performance
{
    [TestFixture]
    public class AllocationPerformanceTest
    {
        private const int _iterations = 1000000;

        [Ignore]
        public void guid_allocation_performance()
        {
            List<Guid> list = new List<Guid>(_iterations);

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _iterations; i++)
            {
                var array = new byte[16] { 1, (byte) i, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };
                var newguid = new Guid(array);
                list.Add(newguid);
            }
            watch.Stop();

            Console.WriteLine("Done in {0} msec", watch.ElapsedMilliseconds);
        }

        [Ignore]
        public void binary_in_container_allocation_performance()
        {
            List<ByteContainer> list = new List<ByteContainer>();

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _iterations; i++)
            {
                var array = new byte[16] { 1, (byte) i,3,4,5,6,7,8,9,0,1,2,3,4,5,6};

                var container = new ByteContainer(array);
                list.Add(container);
            }
            watch.Stop();

            Console.WriteLine("Done in {0} msec", watch.ElapsedMilliseconds);            
        }

        [Ignore]
        public void short_string_in_container_allocation_performance()
        {
            List<StringContainer> list = new List<StringContainer>();

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _iterations; i++)
            {
                var value = String.Format("1234{0}567890123456", i);

                var container = new StringContainer(value);
                list.Add(container);
            }
            watch.Stop();

            Console.WriteLine("Done in {0} msec", watch.ElapsedMilliseconds);            
        }

        [Ignore]
        public void long_string_in_container_allocation_performance()
        {
            List<StringContainer> list = new List<StringContainer>();

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _iterations; i++)
            {
                var value = String.Format("1234{0}567890123456567890123456567890123456567890123456567890123456567890123456567890123456567890123456", i);

                var container = new StringContainer(value);
                list.Add(container);
            }
            watch.Stop();

            Console.WriteLine("Done in {0} msec", watch.ElapsedMilliseconds);
        }

        [Ignore]
        public void different_arrays_should_be_returned_from_guid()
        {
            var guid = Guid.NewGuid();
            var b1 = guid.ToByteArray();
            var b2 = guid.ToByteArray();

            Assert.That(b1 == b2, Is.False);
        }
    }


    #region Helper classes

    public class ByteContainer
    {
        private readonly byte[] _data;

        public ByteContainer(byte[] data)
        {
            _data = data;
        }
    }

    public class StringContainer
    {
        private readonly String value;

        public StringContainer(string value)
        {
            this.value = value;
        }
    }

    #endregion
}
