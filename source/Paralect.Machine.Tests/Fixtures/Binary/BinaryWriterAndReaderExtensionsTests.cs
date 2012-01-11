using System;
using System.IO;
using MongoDB.Bson;
using NUnit.Framework;
using Paralect.Machine.Utils;

namespace Paralect.Machine.Tests.Fixtures.Binary
{
    [TestFixture]
    public class BinaryWriterAndReaderExtensionsTests
    {
        [Test]
        public void should_write_and_read_guid()
        {
            Guid guid = new Guid("{5D9BB68D-8092-4433-8E4A-26AABE83AEF3}");

            Byte[] data = new byte[16];
            MemoryStream stream = new MemoryStream(data);

            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(guid);
            stream.Position = 0;

            BinaryReader reader = new BinaryReader(stream);
            var savedGuid = reader.ReadGuid();

            Assert.That(savedGuid, Is.EqualTo(guid));
        }

        [Test]
        public void should_write_and_read_objectid()
        {
            ObjectId objectId = ObjectId.Parse("4f0a1fc9c5355718e41c9343");

            Byte[] data = new byte[12];
            MemoryStream stream = new MemoryStream(data);

            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(objectId);
            stream.Position = 0;

            BinaryReader reader = new BinaryReader(stream);
            var savedObjectId = reader.ReadObjectId();

            Assert.That(savedObjectId, Is.EqualTo(objectId));
        }

        /// <summary>
        /// Reading and writing strings is supported by BCL, but just to be sure
        /// </summary>
        [Test]
        public void should_write_and_read_string()
        {
            String hello = "Hello";
            String hello2 = "Hello World \0 \t \n\r \r\n";

            Byte[] data = new byte[40];
            MemoryStream stream = new MemoryStream(data);

            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(hello);
            writer.Write(hello2);
            stream.Position = 0;

            BinaryReader reader = new BinaryReader(stream);
            var savedString = reader.ReadString();
            var savedString2 = reader.ReadString();

            Assert.That(savedString, Is.EqualTo(hello));
            Assert.That(savedString2, Is.EqualTo(hello2));
        }
    }
}
