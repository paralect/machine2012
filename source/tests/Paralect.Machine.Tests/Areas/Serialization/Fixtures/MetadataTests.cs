using System.Collections.Generic;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Tests.Areas.Serialization.Fixtures.Protobuf;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    [TestFixture]
    public class MetadataTests
    {
        [Test]
        public void test()
        {
            var context = MachineContext.Create(b => b
                .RegisterIdentities(typeof(UserId)/*, typeof(SuperUserId)*/)
            );

            var metadata = new StateMetadata();

            metadata.ProcessId = new UserId() { Value = "hello" };

            var bytes = ProtobufSerializer.SerializeProtocalBuffer(metadata, context.Serializer.Model);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<StateMetadata>(bytes, context.Serializer.Model);
        }

        [Test]
        public void message_should_work()
        {
            var context = MachineContext.Create(b => b
                .RegisterIdentities(typeof(UserId)/*, typeof(SuperUserId)*/)
            );

            var metadata = new MessageMetadata();

            metadata.Receivers = new List<IIdentity>()
            {
                new UserId() { Value = "hello" },
                new UserId() { Value = "hello232" },
                new UserId() { Value = "hello232dfhdghdgh" },
            };
                
            var bytes = ProtobufSerializer.SerializeProtocalBuffer(metadata, context.Serializer.Model);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<MessageMetadata>(bytes, context.Serializer.Model);            
        }
    }

    [ProtoContract, Identity("{c88b1ca7-a325-41f5-844f-105300124187}")]
    public class UserId : StringId
    {
        
    }

    [ProtoContract, Identity("{d0631888-98cd-45d6-b603-64ca7245818b}")]
    public class SuperUserId : StringId
    {
        
    }
}