using Paralect.Machine.Identities;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    public class EventMetadata : MessageMetadata, IEventMetadata
    {
        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        [ProtoMember(1)]
        public IIdentity SenderId { get; set; }

        /// <summary>
        /// Version of Aggregate Root, Service or Process at the moment event was emitted.
        /// Emitting party should increment version and next event should have version incremented by one.
        /// Can be used to support commutativity of event handling operations.
        /// </summary>
        [ProtoMember(2)]
        public int SenderVersion { get; set; }
    }
}