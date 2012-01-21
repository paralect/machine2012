using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Serialization;
using Paralect.Machine.Tests.Helpers;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    [TestFixture]
    public class ProtobufSerializerTests
    {
        [Test]
        public void should_serialize_simple_event()
        {
            Test(
                new[] { typeof(ProtobufSerializer_Event) }, 
                new[] { typeof(ProtobufSerializer_Id) }, 
                new ProtobufSerializer_Event()
                {
                    Rate = 56.6, Title = "Hello",
                    Metadata = new EventMetadata<ProtobufSerializer_Id>()
                    {
                        LamportTimestamp = 567,
                        MessageId = Guid.NewGuid(),
                        SenderId = new ProtobufSerializer_Id() { Value = "som_id" },
                        SenderVersion = 45,
                        TriggerMessageId = Guid.NewGuid()
                    }
                }
            );
        }

        [Test]
        public void should_serialize_children()
        {
            Test(
                new[] { typeof(ProtobufSerializer_Child_Event) },
                new[] { typeof(ProtobufSerializer_Id) },
                new ProtobufSerializer_Child_Event()
                {
                    Rate = 56.6,
                    Title = "Hello",
                    Child = "Child data",
                    Metadata = new EventMetadata<ProtobufSerializer_Id>()
                    {
                        LamportTimestamp = 567,
                        MessageId = Guid.NewGuid(),
                        SenderId = new ProtobufSerializer_Id() { Value = "som_id" },
                        SenderVersion = 45,
                        TriggerMessageId = Guid.NewGuid()
                    }
                }
            );
        }

        [Test]
        public void should_serialize_base_and_children()
        {
            Test(
                new[] { typeof(ProtobufSerializer_Child_Event), typeof(ProtobufSerializer_Event) },
                new[] { typeof(ProtobufSerializer_Id) },
                new ProtobufSerializer_Child_Event()
                {
                    Rate = 56.6,
                    Title = "Hello",
                    Child = "Child data",
                    Metadata = new EventMetadata<ProtobufSerializer_Id>()
                    {
                        LamportTimestamp = 567,
                        MessageId = Guid.NewGuid(),
                        SenderId = new ProtobufSerializer_Id() { Value = "som_id" },
                        SenderVersion = 45,
                        TriggerMessageId = Guid.NewGuid()
                    }
                },
                new ProtobufSerializer_Event()
                {
                    Rate = 56.6,
                    Title = "Hello",
                    Metadata = new EventMetadata<ProtobufSerializer_Id>()
                    {
                        LamportTimestamp = 567,
                        MessageId = Guid.NewGuid(),
                        SenderId = new ProtobufSerializer_Id() { Value = "som_id" },
                        SenderVersion = 45,
                        TriggerMessageId = Guid.NewGuid()
                    }
                }
            );
        }

        [Test]
        public void should_serialize_two_childrens_of_the_same_base_event()
        {
            Test(
                new[] { typeof(ProtobufSerializer_Child_Event), typeof(ProtobufSerializer_Child2_Event) },
                new[] { typeof(ProtobufSerializer_Id) },
                new ProtobufSerializer_Child_Event()
                {
                    Rate = 56.6,
                    Title = "Hello",
                    Child = "Child data",
                    Metadata = new EventMetadata<ProtobufSerializer_Id>()
                    {
                        LamportTimestamp = 567,
                        MessageId = Guid.NewGuid(),
                        SenderId = new ProtobufSerializer_Id() { Value = "some_id" },
                        SenderVersion = 45,
                        TriggerMessageId = Guid.NewGuid()
                    }
                },                
                new ProtobufSerializer_Child2_Event()
                {
                    Rate = 56.6,
                    Title = "Hello",
                    Child2 = "Child2 data",
                    Metadata = new EventMetadata<ProtobufSerializer_Id>()
                    {
                        LamportTimestamp = 567,
                        MessageId = Guid.NewGuid(),
                        SenderId = new ProtobufSerializer_Id() { Value = "some_id" },
                        SenderVersion = 45,
                        TriggerMessageId = Guid.NewGuid()
                    }
                },
                new ProtobufSerializer_Event()
                {
                    Rate = 56.6,
                    Title = "Hello",
                    Metadata = new EventMetadata<ProtobufSerializer_Id>()
                    {
                        LamportTimestamp = 567,
                        MessageId = Guid.NewGuid(),
                        SenderId = new ProtobufSerializer_Id() { Value = "someee_id" },
                        SenderVersion = 45,
                        TriggerMessageId = Guid.NewGuid()
                    }
                }
            );
        }

        private void Test(IEnumerable<Type> messageTypes, IEnumerable<Type> identityType, params Object[] objects)
        {
            var map = new Dictionary<Object, byte[]>();

            var messageTypeList = messageTypes.ToList();
            var identityTypeList = identityType.ToList();

            var messageFactory = new MessageFactory(messageTypeList);
            var identityFactory = new IdentityFactory(identityTypeList);

            var serializer = new ProtobufSerializer();
            serializer.RegisterMessages(messageFactory.MessageDefinitions);
            serializer.RegisterIdentities(identityFactory.IdentityDefinitions);

            foreach (var obj in objects)
            {
                map[obj] = serializer.Serialize(obj);
            }

            // reverse order of messages and identities
            messageTypeList.Reverse();
            identityTypeList.Reverse();

            messageFactory = new MessageFactory(messageTypeList);
            identityFactory = new IdentityFactory(identityTypeList);

            serializer = new ProtobufSerializer();
            serializer.RegisterMessages(messageFactory.MessageDefinitions);
            serializer.RegisterIdentities(identityFactory.IdentityDefinitions);

            foreach (object obj in objects)
            {
                var back = serializer.Deserialize(map[obj], obj.GetType());
                var result = ObjectComparer.AreObjectsEqual(obj, back);
                Assert.That(result, Is.True);
            }
                

        }



        [Test]
        public void should_be_calculated_correctly()
        {
            Action<String, Int32> test = (guidText, tag) =>
            {
                var guid = Guid.Parse(guidText);
                var calculatedTag = ProtobufSerializer.GenerateHierarchyTag(guid);
                Assert.That(calculatedTag, Is.EqualTo(tag));
            };

            test("{66047c12-32c5-4842-8023-70d1658a836e}", 12661822);
            test("{ea1dbd75-bf2b-4ed9-be6a-ce32c6b3e505}", 13175895);
            test("{decde923-ff46-4c20-a6f6-ac7645e5325b}",  7625384);
            test("{5c69c886-76ef-4458-8afb-dc7cc40501a2}",  7959428);
            test("{628f6c9b-99bb-4bda-90e5-7b406ca0271a}", 13359269);
        }
    }


    [ProtoContract, Identity("{7a0e638e-4d91-4216-aeb0-3bb4584e64d4}")]
    public class ProtobufSerializer_Id : StringId { }

    [ProtoContract, Message("{31cbf70a-5449-4aae-b086-e41b0fa69acc}")]
    public class ProtobufSerializer_Event : Event<ProtobufSerializer_Id>
    {
        [ProtoMember(1)]
        public String Title { get; set; }

        [ProtoMember(2)]
        public Double Rate { get; set; }
    }

    [ProtoContract, Message("{24a94fe0-458c-443a-8bee-a293dfc8eb46}")]
    public class ProtobufSerializer_Child_Event : ProtobufSerializer_Event
    {
        [ProtoMember(1)]
        public String Child { get; set; }
    }

    [ProtoContract, Message("{6d828686-fcdd-4d4d-9daa-3c18d205f647}")]
    public class ProtobufSerializer_Child2_Event : ProtobufSerializer_Event
    {
        [ProtoMember(1)]
        public String Child2 { get; set; }
    }
}