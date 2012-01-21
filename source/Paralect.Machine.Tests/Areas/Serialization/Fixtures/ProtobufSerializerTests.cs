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
        public void should_serialize_basic_list_and_dictionary_in_event()
        {
            Test(
                new[] { typeof(ProtobufSerializer_ListAndDictionaryEvent)},
                new[] { typeof(ProtobufSerializer_Id) },
                new ProtobufSerializer_ListAndDictionaryEvent()
                {
                    Rate = 56.6,
                    Title = "Hello",
                    Child2 = "Child data",
                    Dictionary = new Dictionary<string, int>() { {"hello", 1}, {"bye", 2} },
                    List = new List<string>() { "minsk", "moscow", "paris" },
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
        public void should_throw_on_collision()
        {
            Assert.Throws<ProtoHierarchyTagCollision>(() =>
            {
                Test(
                    new[] { typeof(ProtobufSerializer_CollisionEvent), typeof(ProtobufSerializer_CollisionEvent2) },
                    new[] { typeof(ProtobufSerializer_Id) },
                    new ProtobufSerializer_CollisionEvent()
                    {
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
            });
        }

        [Test]
        public void should_not_throw_on_collision()
        {
            //
            // this should work because messages have different parent types (thus types are in different hierarchy)
            //

            Test(
                new[] { typeof(ProtobufSerializer_NoCollisionEvent), typeof(ProtobufSerializer_NoCollisionEvent2) },
                new[] { typeof(ProtobufSerializer_Id) },
                new ProtobufSerializer_NoCollisionEvent()
                {
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
        public void hierarchy_tag_should_be_calculated_correctly()
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


    #region Identities and messages

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

    [ProtoContract, Message("{9abb67d4-b83d-42c1-8bbc-c20ffaff9fa5}")]
    public class ProtobufSerializer_ListAndDictionaryEvent : ProtobufSerializer_Child2_Event
    {
        [ProtoMember(1)]
        public List<String> List { get; set; }

        [ProtoMember(2)]
        public Dictionary<String, Int32> Dictionary { get; set; }
    }

    #endregion

    #region Protobuf tags collision

    [ProtoContract]
    [Message("{c88b1ca7-a325-41f5-844f-105300124187}")]
    public class ProtobufSerializer_CollisionEvent : Event<ProtobufSerializer_Id>
    {
        [ProtoMember(1)]
        public string Name { get; set; }
    }

    [ProtoContract]
    [Message("{d0631888-98cd-45d6-b603-64ca7245818b}")]
    public class ProtobufSerializer_CollisionEvent2 : Event<ProtobufSerializer_Id>
    {
        [ProtoMember(1)]
        public string ChildName { get; set; }
    }

    #endregion

    #region Protobuf no collision (because different hierarchy)

    [ProtoContract]
    [Message("{c88b1ca7-a325-41f5-844f-105300124187}")]
    public class ProtobufSerializer_NoCollisionEvent : Event<ProtobufSerializer_Id>
    {
        [ProtoMember(1)]
        public string Name { get; set; }
    }

    [ProtoContract]
    [Message("{d0631888-98cd-45d6-b603-64ca7245818b}")]
    public class ProtobufSerializer_NoCollisionEvent2 : ProtobufSerializer_Child2_Event
    {
        [ProtoMember(1)]
        public string ChildName { get; set; }
    }

    #endregion


}