using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paralect.Machine.Utils;

namespace Paralect.Machine.Tests
{
    public class ByteArrayComparerTests
    {
        private readonly Func<byte[]> _createArrayFunc = () => new byte[] { 1, 2, 3, 4, 5 };
        private const String _value = "Some Value";

        [Test]
        public void should_fail_if_without_comparer()
        {
            Dictionary<byte[], string> dictionary = new Dictionary<byte[], string>();
            dictionary[_createArrayFunc()] = _value;

            Assert.Throws<KeyNotFoundException>(() =>
            {
                string value = dictionary[_createArrayFunc()];
            });
        }

        [Test]
        public void should_succeed_if_with_comparer()
        {
            Dictionary<byte[], string> dictionary = new Dictionary<byte[], string>(new ByteArrayComparer());
            dictionary[_createArrayFunc()] = _value;

            string value = dictionary[_createArrayFunc()];

            Assert.That(value, Is.EqualTo(_value));
        }      
    }
}