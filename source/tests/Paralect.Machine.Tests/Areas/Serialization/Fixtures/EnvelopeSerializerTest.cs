using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Serialization;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    public class EnvelopeSerializerTest
    {
        [Test]
        public void Teststtt()
        {
            var messageFactory = new MessageFactory(typeof(EnvelopeSerializer_Event), typeof(EnvelopeSerializer_Child_Event));
            var identityFactory = new IdentityFactory(typeof(EnvelopeSerializer_Id));

            var serializer = new ProtobufSerializer();
            serializer.RegisterMessages(messageFactory.MessageDefinitions);
            serializer.RegisterIdentities(identityFactory.IdentityDefinitions);

            var message1 = new EnvelopeSerializer_Event()
            {
                Rate = 0.7,
                Title = "Muahaha!"
            };

            var envelope = new EnvelopeBuilder(messageFactory.TypeToTagResolver)
                .AddMessage(message1)
                .Build();

            var envelopeSerializer = new EnvelopeSerializer(serializer, messageFactory.TagToTypeResolver);
            var bytes = envelopeSerializer.Serialize(envelope);
            var back = envelopeSerializer.Deserialize(bytes);

            var evnt = (EnvelopeSerializer_Event) back.Items.First().Message;

            Assert.That(evnt.Rate, Is.EqualTo(message1.Rate));
            Assert.That(evnt.Title, Is.EqualTo(message1.Title));
        }
    }

    #region helpers

    [ProtoContract, Identity("{803b41b9-b566-4baf-a5ea-9744959fbac7}")]
    public class EnvelopeSerializer_Id : StringId { }

    [ProtoContract, Message("{74467730-33c0-418a-bd83-963258ce6fa9}")]
    public class EnvelopeSerializer_Event : IEvent<ProtobufSerializer_Id>
    {
        [ProtoMember(1)]
        public String Title { get; set; }

        [ProtoMember(2)]
        public Double Rate { get; set; }
    }

    [ProtoContract, Message("{f55856e9-66b3-4fd4-9f6a-de9c2606a692}")]
    public class EnvelopeSerializer_Child_Event : ProtobufSerializer_Event
    {
        [ProtoMember(1)]
        public String Child { get; set; }
    }

    #endregion
}