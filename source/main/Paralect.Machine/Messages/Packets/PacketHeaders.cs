using System;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    public class PacketHeaders : IPacketHeaders
    {
        [ProtoMember(1)]
        public ContentType ContentType { get; set; }

        [ProtoMember(2)]
        public Int64 PreviousJournalSequence { get; set; }

        [ProtoMember(3)]
        public Int64 CurrentJournalSequence { get; set; }
    }
}