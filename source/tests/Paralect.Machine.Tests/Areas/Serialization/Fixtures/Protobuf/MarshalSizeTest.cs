using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures.Protobuf
{
    [TestFixture]
    public class MarshalSizeTest
    {
        [Test]
        public void do_tests()
        {
            var obj = new SimpleClass();
            //obj.Var = 0;
            obj.D = DateTime.MinValue;

            var bytes = ProtobufSerializer.SerializeProtocalBuffer(obj);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<SimpleClass>(bytes);
            
        }
    }

    [ProtoContract]
    public class SimpleClass
    {
        [ProtoMember(1)]
        public List<Int32> Var { get; set; }

        [ProtoMember(2)]
        public List<Guid> G { get; set; }

        [ProtoMember(3)]
        public DateTime D { get; set; }

    }

}