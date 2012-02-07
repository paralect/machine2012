using System;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Tests.Helpers;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    [TestFixture]
    public class PacketSerializationTests
    {
        [Test]
        public void should_correctly_serialize_and_deserialize()
        {
            var context = CreateContext();
            var message = new PacketSerialization_Event() { Rate = 66, Title = "Hello" };
            var packet = context.CreatePacket(b => b.AddMessage(message));

            var parts = packet.Serialize();

            var back = context.CreatePacket(parts);

            var backMessage = back.GetEnvelopes()[0].GetMessage();

            Assert.IsFalse(backMessage == message);
            AssertObjectsEqual(backMessage, message);
        }        
        
        [Test]
        public void should_correctly_toggle_source_of_headers()
        {
            var context = CreateContext();
            var message = new PacketSerialization_Event() { Rate = 66, Title = "Hello" };
            var packet = context.CreatePacket(b => b.AddMessage(message));

            packet.GetHeadersBinary();
            packet.GetHeaders();

            var header = packet.GetHeaders();
            header.ContentType = ContentType.States;

            var parts = packet.Serialize();

            var back = context.CreatePacket(parts);

            var backHeaders = back.GetHeaders();
            var backMessage = back.GetEnvelopes()[0].GetMessage();

            Assert.IsFalse(backHeaders == header);
            AssertObjectsEqual(backHeaders, header);

            Assert.IsFalse(backMessage == message);
            AssertObjectsEqual(backMessage, message);
        }

        [Test]
        public void should_correctly_clone_in_unserialized_mode()
        {
            var context = CreateContext();
            var message = new PacketSerialization_Event() { Rate = 66, Title = "Hello" };
            var packet = context.CreatePacket(b => b.AddMessage(message));

            var original = packet.GetEnvelopes()[0];
            var cloned = packet.GetEnvelopesCopy()[0];

            Assert.IsFalse(original == cloned);
            Assert.IsFalse(original.GetMessage() == cloned.GetMessage());
            Assert.IsFalse(original.GetMetadata() == cloned.GetMetadata());
            AssertObjectsEqual(original.GetMessage(), cloned.GetMessage());
            AssertObjectsEqual(original.GetMetadata(), cloned.GetMetadata());
        }

        [Test]
        public void should_correctly_clone_in_serialized_mode()
        {
            var context = CreateContext();
            var message = new PacketSerialization_Event() { Rate = 666, Title = "Hello66" };
            var packet = context.CreatePacket(b => b.AddMessage(message));

            var parts = packet.Serialize();
            var back = context.CreatePacket(parts);

            var original = back.GetEnvelopes()[0];
            var cloned = back.GetEnvelopesCopy()[0];

            Assert.IsFalse(original == cloned);
            Assert.IsFalse(original.GetMessage() == cloned.GetMessage());
            Assert.IsFalse(original.GetMetadata() == cloned.GetMetadata());
            AssertObjectsEqual(original.GetMessage(), cloned.GetMessage());
            AssertObjectsEqual(original.GetMetadata(), cloned.GetMetadata());
        }

        private void AssertObjectsEqual(object one, object two)
        {
            var result = ObjectComparer.AreObjectsEqual(one, two);
            Assert.IsTrue(result);
        }

        private MachineContext CreateContext()
        {
            return MachineContext.Create(b => b
                .RegisterIdentities(typeof(PacketSerialization_Id))
                .RegisterMessages(typeof(PacketSerialization_Event), typeof(PacketSerialization_Child_Event))
            );
        }
    }

    #region Identities and messages

    [ProtoContract, Identity("{7a0e638e-4d91-4216-aeb0-3bb4584e64d4}")]
    public class PacketSerialization_Id : StringId { }

    [ProtoContract, Message("{31cbf70a-5449-4aae-b086-e41b0fb69acc}")]
    public class PacketSerialization_Event : IEvent<PacketSerialization_Id>
    {
        [ProtoMember(1)]
        public String Title { get; set; }

        [ProtoMember(2)]
        public Double Rate { get; set; }
    }

    [ProtoContract, Message("{24a94fe0-458c-443a-8bee-a293dfc8eb46}")]
    public class PacketSerialization_Child_Event : PacketSerialization_Event
    {
        [ProtoMember(1)]
        public String Child { get; set; }
    }

    #endregion
}

