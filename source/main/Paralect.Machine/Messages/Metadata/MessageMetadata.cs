using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    [ProtoInclude(501, typeof(EventMetadata))]
    [ProtoInclude(502, typeof(CommandMetadata))]
    public class MessageMetadata : IMessageMetadata
    {
        [ProtoMember(1)]
        public Guid MessageId { get; set; }

        [ProtoMember(2)]
        public Guid TriggerMessageId { get; set; }

        [ProtoMember(3)]
        public IList<IIdentity> Receivers { get; set; }

        [ProtoMember(4)]
        public long LamportTimestamp { get; set; }

        [ProtoMember(5)]
        public DateTime DeliverOnUtc { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedOnUtc { get; set; }

        [ProtoMember(7)]
        public Guid MessageTag { get; set; }

        [ProtoMember(8)]
        public Int64 JournalSequence { get; set; }

        [ProtoMember(9)]
        public Dictionary<String, Int32> Partitions { get; set; } 
    }
}