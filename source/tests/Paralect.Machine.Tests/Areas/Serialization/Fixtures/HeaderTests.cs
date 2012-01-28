using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Paralect.Machine.Serialization;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    [TestFixture]
    public class HeaderTests
    {
        [Test]
        public void simple_test()
        {
            var header = new Header();
            header.Set("Hello", "World");
            
            var serializer = new ProtobufSerializer();

            var bytes = serializer.Serialize(header);
            var back = serializer.Deserialize<Header>(bytes);

            Assert.That(back.GetString("Hello"), Is.EqualTo("World"));
            Assert.That(back.GetString("Hello"), Is.EqualTo(header.GetString("Hello")));
        }

        [Test]
        public void emtpy_or_just_created_header_should_take_zero_bytes_when_serialized()
        {
            var header = new Header();
            var serializer = new ProtobufSerializer();

            var bytes = serializer.Serialize(header);
            var back = serializer.Deserialize<Header>(bytes);

            Assert.That(bytes.Length, Is.EqualTo(0));
        }

        [Test]
        public void should_correctly_performs_with_string_values()
        {
            var header = new Header();

            header.Set("Test", "Value");
            header.Set("Test2", "Value2");

            Assert.That(header.ContainsString("Test"), Is.EqualTo(true));
            Assert.That(header.ContainsString("Test2"), Is.EqualTo(true));
            Assert.That(header.ContainsString("Test3"), Is.EqualTo(false));

            Assert.That(header.GetString("Test"), Is.EqualTo("Value"));
            Assert.That(header.GetString("Test2"), Is.EqualTo("Value2"));

            Assert.Throws<KeyNotFoundException>(() => { header.GetString("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetInt32("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetInt64("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetBoolean("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetDateTime("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetGuid("Muhahaha"); });

            Assert.Throws<InvalidCastException>(() => { header.GetInt32("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetInt64("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetBoolean("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetGuid("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetDateTime("Test"); });

            String value;
            Boolean result;

            result = header.TryGetString("Test", out value);
            Assert.That(result, Is.EqualTo(true));
            Assert.That(value, Is.EqualTo("Value"));

            result = header.TryGetString("Test2", out value);
            Assert.That(result, Is.EqualTo(true));
            Assert.That(value, Is.EqualTo("Value2"));

            result = header.TryGetString("Test3", out value);
            Assert.That(result, Is.EqualTo(false));
            Assert.That(value, Is.EqualTo(null));

            Int32 intValue;
            result = header.TryGetInt32("Test3", out intValue);
            Assert.That(result, Is.EqualTo(false));
            Assert.That(intValue, Is.EqualTo(0));
        }


        [Test]
        public void should_correctly_performs_with_int32_values()
        {
            var header = new Header();

            header.Set("Test", 35);
            header.Set("Test2", 67);

            Assert.That(header.ContainsInt32("Test"), Is.EqualTo(true));
            Assert.That(header.ContainsInt32("Test2"), Is.EqualTo(true));
            Assert.That(header.ContainsInt32("Test3"), Is.EqualTo(false));

            Assert.That(header.GetInt32("Test"), Is.EqualTo(35));
            Assert.That(header.GetInt32("Test2"), Is.EqualTo(67));

            Assert.Throws<KeyNotFoundException>(() => { header.GetString("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetInt32("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetInt64("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetBoolean("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetDateTime("Muhahaha"); });
            Assert.Throws<KeyNotFoundException>(() => { header.GetGuid("Muhahaha"); });

            Assert.Throws<InvalidCastException>(() => { header.GetString("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetInt64("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetBoolean("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetGuid("Test"); });
            Assert.Throws<InvalidCastException>(() => { header.GetDateTime("Test"); });

            Int32 value;
            Boolean result;

            result = header.TryGetInt32("Test", out value);
            Assert.That(result, Is.EqualTo(true));
            Assert.That(value, Is.EqualTo(35));

            result = header.TryGetInt32("Test2", out value);
            Assert.That(result, Is.EqualTo(true));
            Assert.That(value, Is.EqualTo(67));

            result = header.TryGetInt32("Test3", out value);
            Assert.That(result, Is.EqualTo(false));
            Assert.That(value, Is.EqualTo(0));
        }

        [Test]
        public void should_not_allow_insert_null_values_and_keys()
        {
            var header = new Header();

            Assert.Throws<ArgumentNullException>(() => { header.Set("Test", null); });
            Assert.Throws<ArgumentNullException>(() => { header.Set(null, "Test"); });
            Assert.Throws<ArgumentNullException>(() => { header.Set(null, null); });
            
        }
    }
}