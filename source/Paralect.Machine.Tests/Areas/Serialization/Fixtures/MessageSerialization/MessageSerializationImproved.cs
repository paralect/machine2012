using System;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Serialization;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures.MessageSerialization
{
    [TestFixture]
    public class MessageSerializationImproved
    {
        [Test]
        public void test_serialization()
        {
            var messFactory = new MessageFactory(typeof(ChildSampleEvent), typeof(Child2SampleEvent));
            var protoSerializer = new ProtobufSerializer();
            protoSerializer.RegisterMessages(messFactory.MessageDefinitions);

            var evnt = new Child2SampleEvent() { Name = "Dima", Year = "2012", Child2Name = "Child", Child2Year = "3033" };
            evnt.Metadata = new EventMetadata<SampleId>() { MessageId = Guid.NewGuid(), SenderVersion = 45 };

            var bytes = protoSerializer.Serialize(evnt);
            var back = protoSerializer.Deserialize<Child2SampleEvent>(bytes);



        }
    }



    [ProtoContract]
    public class SampleId : StringId
    {

    }

    [ProtoContract]
    [Message("{450e5ef4-e623-4775-b8f4-129698e19e22}")]
    public class SampleEvent<TID> : Event<TID>
        where TID : IIdentity
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public string Year { get; set; }
    }

    [ProtoContract]
    [Message("{8099fc8d-bccd-44c9-a641-0af70c2b27a1}")]
    public class ChildSampleEvent : SampleEvent<SampleId>
    {
        [ProtoMember(1)]
        public string ChildName { get; set; }
        [ProtoMember(2)]
        public string ChildYear { get; set; }
    }

    [ProtoContract]
    [Message("{8099fc8d-bccd-44c9-a641-0af70c2b27a2}")]
    public class Child2SampleEvent : SampleEvent<SampleId>
    {
        [ProtoMember(1)]
        public string Child2Name { get; set; }
        [ProtoMember(2)]
        public string Child2Year { get; set; }
    }
}