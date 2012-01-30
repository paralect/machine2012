using System;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Tests.Areas.Serialization.Fixtures.Protobuf;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures.MessageSerialization
{
    [TestFixture]
    public class MessageSupportedSerializationTest
    {
        [Test]
        public void test_serialization()
        {
            var evnt = new MyEvent() { Name = "Dima", Year = "2012" };
            //evnt.Metadata = new EventMetadata<MyProcessId>() { MessageId = Guid.NewGuid(), SenderVersion = 45 };

            
            var model = TypeModel.Create();

/*            model[typeof(IIdentity)]
                .AddSubType(100, typeof(IIdentity<String>));

            model[typeof(IIdentity<String>)]
                .AddSubType(100, typeof(StringId));

            model[typeof(StringId)]
                .AddSubType(100, typeof(MyProcessId));*/



/*            model[typeof(IMessageMetadata)]
                .AddSubType(100, typeof(IEventMetadata));

            model[typeof(IEventMetadata)]
                .AddSubType(100, typeof(IEventMetadata<MyProcessId>));

            model[typeof(IEventMetadata<MyProcessId>)]
                .AddSubType(100, typeof(EventMetadata<MyProcessId>));*/


/*            model[typeof(IMessage)]
                .AddSubType(100, typeof(IEvent));

            model[typeof(IEvent)]
                .AddSubType(100, typeof(IEvent<MyProcessId>));

            model[typeof(IEvent<MyProcessId>)]
                .AddSubType(100, typeof(Event<MyProcessId>));
*/

            model[typeof(IEvent<MyProcessId>)]
                .AddSubType(100, typeof(MyEvent));

            //model.Add(typeof(Event<MyProcessId>), true);

            var bytes = ProtobufSerializer.SerializeProtocalBuffer(evnt, model);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<MyEvent>(bytes, model);

            Assert.That(back.Name, Is.EqualTo(evnt.Name));
            Assert.That(back.Year, Is.EqualTo(evnt.Year));
            
        }
    }

    [ProtoContract]
    public class MyProcessId : StringId
    {
        
    }

    [Message("{450e5ef4-e623-4775-b8f4-129698e19e22}")]
    [ProtoContract]
    public class MyEvent : IEvent<MyProcessId>
    {
        [ProtoMember(1)] public string Name { get; set; }
        [ProtoMember(2)] public string Year { get; set; }
    }
}