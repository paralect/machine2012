using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    [TestFixture]
    public class MessageFactoryTests
    {
        [Test]
        public void should_throw_on_duplicated_tag()
        {
            Assert.Throws<MessageTagAlreadyRegistered>(() =>
            {
                new MessageFactory(typeof(MessageFactoryEvent), typeof(MessageFactoryEvent2));
            });
        }

        [Test]
        public void should_throw_on_duplicated_registration_of_the_same_message()
        {
            Assert.Throws<MessageTypeAlreadyRegistered>(() =>
            {
                new MessageFactory(typeof(MessageFactoryEvent), typeof(MessageFactoryEvent));
            });
        }

        [Test]
        public void should_throw_if_protobuf_hierarchy_tag_collision_found()
        {
            Assert.Throws<MessageProtoHierarchyTagCollision>(() =>
            {
                new MessageFactory(typeof(MessageFactoryProtoCollisionEvent), typeof(MessageFactoryProtoCollisionEvent2));
            });            
        }

        [Test]
        public void should_throw_if_no_message_attribute_for_message_type()
        {
            Assert.Throws<MessageTagNotSpecified>(() =>
            {
                new MessageFactory(typeof(MessageWithoutAttribute));
            });                        
        }
    }


    [ProtoContract]
    public class MessageFactoryProcessId : StringId { }

    #region Duplicate tag

    [ProtoContract]
    [Message("{509bc2f6-dc16-408d-a083-b4982e866034}")]
    public class MessageFactoryEvent : Event<MessageFactoryProcessId>
    {
        [ProtoMember(1)]
        public string Name { get; set; }
    }

    [ProtoContract]
    [Message("{509bc2f6-dc16-408d-a083-b4982e866034}")]
    public class MessageFactoryEvent2 : Event<MessageFactoryProcessId>
    {
        [ProtoMember(1)]
        public string ChildName { get; set; }
    }

    #endregion

    #region Protobuf tags collision

    [ProtoContract]
    [Message("{c88b1ca7-a325-41f5-844f-105300124187}")]
    public class MessageFactoryProtoCollisionEvent : Event<MessageFactoryProcessId>
    {
        [ProtoMember(1)]
        public string Name { get; set; }
    }

    [ProtoContract]
    [Message("{d0631888-98cd-45d6-b603-64ca7245818b}")]
    public class MessageFactoryProtoCollisionEvent2 : Event<MessageFactoryProcessId>
    {
        [ProtoMember(1)]
        public string ChildName { get; set; }
    }

    #endregion

    #region Message not decorated with MessageAttribute

    [ProtoContract]
    public class MessageWithoutAttribute : Event<MessageFactoryProcessId>
    {
        [ProtoMember(1)]
        public string ChildName { get; set; }
    }

    #endregion

}