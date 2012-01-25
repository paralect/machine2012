using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Serialization;
using Paralect.Machine.Transitions;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    [TestFixture]
    public class TransitionEnvelopeSerializerTests
    {
        [Test]
        public void Teststtt()
        {
            var messageFactory = new MessageFactory(typeof(TransitionEnvelopeSerializer_Event), typeof(TransitionEnvelopeSerializer_Child_Event));
            var identityFactory = new IdentityFactory(typeof(TransitionEnvelopeSerializer_Id));

            var serializer = new ProtobufSerializer();
            serializer.RegisterMessages(messageFactory.MessageDefinitions);
            serializer.RegisterIdentities(identityFactory.IdentityDefinitions);

            var header = new TransitionEnvelopeDataHeader()
            {
                MessageTags = new[]
                {
                    Guid.Parse("74467730-33c0-418a-bd83-963258ce6fa9"),
                    Guid.Parse("f55856e9-66b3-4fd4-9f6a-de9c2606a692")
                }
            };

            var message1 = new TransitionEnvelopeSerializer_Event()
            {
                Rate = 0.7,
                Title = "Muahaha!"
            };

            var memory = new MemoryStream();
            serializer.Model.SerializeWithLengthPrefix(memory, header, header.GetType(), PrefixStyle.Base128, 0);
            serializer.Model.SerializeWithLengthPrefix(memory, message1, message1.GetType(), PrefixStyle.Fixed32, 0);

            memory.Position = 0;

            var back = serializer.Model.DeserializeWithLengthPrefix(memory, null, header.GetType(), PrefixStyle.Base128, 0, null);
            var backMessage1 = serializer.Model.DeserializeWithLengthPrefix(memory, null, typeof(TransitionEnvelopeSerializer_Event), PrefixStyle.Fixed32, 0, null);

        }

        private const int _iterations = 100000;

        [Ignore("Takes time. Run this test manually")]
        public void tra_ta_ta()
        {
            var messageFactory = new MessageFactory(typeof(TransitionEnvelopeSerializer_Event), typeof(TransitionEnvelopeSerializer_Child_Event));
            var identityFactory = new IdentityFactory(typeof(TransitionEnvelopeSerializer_Id));

            var serializer = new ProtobufSerializer();
            serializer.RegisterMessages(messageFactory.MessageDefinitions);
            serializer.RegisterIdentities(identityFactory.IdentityDefinitions);

            var message1 = new TransitionEnvelopeSerializer_Event()
            {
                Rate = 0.7,
                Title = "Muahaha!"
            };


            var transition = new Transition(null, DateTime.Now, new List<IEvent>() { message1, message1, message1, message1,message1, message1,message1,message1 }, null);

            var watch = Stopwatch.StartNew();
            for ( int i = 0; i < _iterations; i++)
            {
                var envelopeSerializer = new TransitionEnvelopeSerializer();
                var envelope = envelopeSerializer.Serialize(messageFactory, serializer, transition);

                var doc = Serialize(envelope);

                var envelopeBack = (TransitionEnvelope) Deserialize(doc, typeof(TransitionEnvelope));
                var back = envelopeSerializer.Deserialize(messageFactory, serializer, envelopeBack);
                
            }
            watch.Stop();
            Console.WriteLine("ProtoBuf. In {0} iterations takes {1} ms", _iterations, watch.ElapsedMilliseconds);

//            Assert.That(back.Events.Count, Is.EqualTo(transition.Events.Count));

        }

        [Ignore("Takes time. Run this test manually")]
        public void mongo_rongo()
        {
            var message1 = new TransitionEnvelopeSerializer_Event()
            {
                Rate = 0.7,
                Title = "Muahaha!"
            };


            var transition = new Transition(null, DateTime.Now, new List<IEvent>() { message1, message1, message1, message1, message1, message1, message1, message1 }, null);

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _iterations; i++)
            {
                var bson = Serialize(transition);
                //var text = bson.ToJson();

                var back = Deserialize(bson, typeof(Transition));
            }
            watch.Stop();
            Console.WriteLine("Bson. In {0} iterations takes {1} ms", _iterations, watch.ElapsedMilliseconds);            
        }

        public Object Deserialize(BsonDocument doc, Type type)
        {
            return BsonSerializer.Deserialize(doc, type);
        }

        public BsonDocument Serialize(Object obj)
        {
            BsonDocument data = new BsonDocument();

            var writer = BsonWriter.Create(data);
            BsonSerializer.Serialize(writer, obj.GetType(), obj);

            return data;
        }


    }

    #region helpers

    [ProtoContract, Identity("{803b41b9-b566-4baf-a5ea-9744959fbac7}")]
    public class TransitionEnvelopeSerializer_Id : StringId { }

    [ProtoContract, Message("{74467730-33c0-418a-bd83-963258ce6fa9}")]
    public class TransitionEnvelopeSerializer_Event : Event<ProtobufSerializer_Id>
    {
        [ProtoMember(1)]
        public String Title { get; set; }

        [ProtoMember(2)]
        public Double Rate { get; set; }
    }

    [ProtoContract, Message("{f55856e9-66b3-4fd4-9f6a-de9c2606a692}")]
    public class TransitionEnvelopeSerializer_Child_Event : ProtobufSerializer_Event
    {
        [ProtoMember(1)]
        public String Child { get; set; }
    }

    #endregion
}