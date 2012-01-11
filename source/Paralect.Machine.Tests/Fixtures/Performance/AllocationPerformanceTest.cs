using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Paralect.Machine.Tests.Fixtures.Performance
{
    class Container
    {
        private readonly byte[] _data;

        public Container(byte[] data)
        {
            _data = data;
        }
    }

    [TestFixture]
    public class AllocationPerformanceTest
    {
        private const int _size = 10000000;

        public void bytes()
        {
            var guid = Guid.NewGuid();
            var b1 = guid.ToByteArray();
            var b2 = guid.ToByteArray();

            Assert.That(b1 == b2, Is.False);
        }

        public void guid_allocation()
        {
            List<Guid> list = new List<Guid>(_size);

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _size; i++)
            {
                var array = new byte[16] { 1, (byte) i, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };
                var newguid = new Guid(array);
                list.Add(newguid);
            }
            watch.Stop();

            Console.WriteLine("Done in {0} msec", watch.ElapsedMilliseconds);
        }

        public void binary_in_container()
        {
            List<Container> list = new List<Container>();

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _size; i++)
            {
                var array = new byte[16] { 1, (byte) i,3,4,5,6,7,8,9,0,1,2,3,4,5,6};

                var container = new Container(array);
                list.Add(container);
            }
            watch.Stop();

            Console.WriteLine("Done in {0} msec", watch.ElapsedMilliseconds);            
        }
    }
}
