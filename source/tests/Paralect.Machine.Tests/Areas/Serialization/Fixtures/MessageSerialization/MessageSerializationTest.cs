using System;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Tests.Areas.Serialization.Fixtures.Protobuf;
using Paralect.Machine.Tests.Helpers;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures.MessageSerialization
{
    [TestFixture]
    public class MessageSerializationTest
    {
        [Test]
        public void should_serialize_and_deserialize_message()
        {
            var message = new Message()
            {
                Metadata = new MessageMetadata()
                {
                    LamportTimestamp = 34,
                    MessageId = Guid.NewGuid(),
                    TriggerMessageId = Guid.NewGuid()
                }
            };

            var model = TypeModel.Create();
            model[typeof(IMessageMetadata)]
                .AddSubType(10, typeof(MessageMetadata));

            var bytes = ProtobufSerializer.SerializeProtocalBuffer(message, model);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<Message>(bytes, model);

            Assert.That(back.Metadata.LamportTimestamp, Is.EqualTo(message.Metadata.LamportTimestamp));
            Assert.That(back.Metadata.MessageId, Is.EqualTo(message.Metadata.MessageId));
            Assert.That(back.Metadata.TriggerMessageId, Is.EqualTo(message.Metadata.TriggerMessageId));
        }
    }

    [ProtoContract]
    public class MessageMetadata : IMessageMetadata
    {
        [ProtoMember(1)]
        public Guid MessageId { get; set; }

        [ProtoMember(2)]
        public Guid TriggerMessageId { get; set; }

        [ProtoMember(3)]
        public long LamportTimestamp { get; set; }
    }

    [ProtoContract]
    public class Message : IMessage
    {
        private MessageMetadata _metadata = new MessageMetadata();

        [ProtoMember(1)]
        public IMessageMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = (MessageMetadata) value; }
        }
    }
}