using System;
using NUnit.Framework;

namespace Paralect.Machine.Tests
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
            Do(16777216, 255);
        }


        private void Do(UInt32 tag, Byte version)
        {
            UInt32 packedValue = Pack(tag, version);
            var unpacked = Unpack(packedValue);

            Assert.That(unpacked.Item1, Is.EqualTo(tag));
            Assert.That(unpacked.Item2, Is.EqualTo(version));            
        }

        private UInt32 Pack(UInt32 tag, Byte version)
        {
            byte[] bytes = BitConverter.GetBytes(tag);
            bytes[3] = (byte)((byte)bytes[3] | (byte)version);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private Tuple<UInt32, Byte> Unpack(UInt32 packedValue)
        {
            byte[] bytes = BitConverter.GetBytes(packedValue);
            byte version = bytes[3];
            bytes[3] = 0;
            UInt32 tag = BitConverter.ToUInt32(bytes, 0);
            return new Tuple<uint, byte>(tag, version);
        }
    }
}