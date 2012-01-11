using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Paralect.Machine.Tests.Fixtures.Binary
{
    [TestFixture]
    public class BytesTest
    {
        [Test]
        public void simple() { Do(1256, 6); }

        [Test]
        public void zero() { Do(0, 0); }

        [Test]
        public void max() { Do(16777215, 255); }

        [Test]
        public void invalid()
        {
            //Do(16777216, 255);
        }

        private void Do(Int32 tag, Byte version)
        {
            Int32 packedValue = PackV2(tag, version);
            var unpacked = UnpackV2(packedValue);

//            Assert.That(unpacked.Item1, Is.EqualTo(tag));
//            Assert.That(unpacked.Item2, Is.EqualTo(version));            
        }

        private Int32 PackV2(Int32 tag, Byte version)
        {
            Int32 v = version;
            Int32 result = tag | (v << 24);
            return result;
        }

        private const UInt32 z = 0xff000000;
        private const UInt32 notz = 0x00ffffff;

        private Tuple<Int32, Byte> UnpackV2(Int32 packedValue)
        {
            Int32 v = packedValue >> 24;
            Int32 t = (packedValue << 8) >> 8; // zero top most byte
            //UInt32 t = (packedValue & notz); // zero top most byte
            return new Tuple<int, byte>(t, (byte) v);
        }

        private const int _numberOfIterations = 1000000;

        public void PerformanceTestV2()
        {
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _numberOfIterations; i++)
            {
                Do(56, 45);
            }

            Console.WriteLine("V1: Time: {0}. Iterations: {1}", watch.ElapsedMilliseconds, _numberOfIterations);
        }

        private void Profile(Action action, String what = null)
        {
            var watch = Stopwatch.StartNew();
            action();
            watch.Stop();
            Console.WriteLine("{0}. Time: {1}.", what, watch.ElapsedMilliseconds);
        }



        [Test]
        public void IntVsUInt()
        {
            Console.WriteLine(BitConverter.IsLittleEndian);

            Int32 one = 56;
            var one_ = BitConverter.GetBytes(one);

            Int32 none = -56;
            var none_ = BitConverter.GetBytes(none);

            UInt32 zzz = (UInt32) none;
            var zzznone_ = BitConverter.GetBytes(zzz);

            var shift = none << 8;
            var shiftnone_ = BitConverter.GetBytes(shift);

            var neg = ~none;
            var none__ = BitConverter.GetBytes(neg);
        }


    }
}