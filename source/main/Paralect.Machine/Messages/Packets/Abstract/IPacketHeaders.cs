using System;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    public interface IPacketHeaders
    {
        ContentType ContentType { get; set; }

        [ProtoMember(2)]
        Int64 PreviousJournalSequence { get; set; }

        [ProtoMember(3)]
        Int64 CurrentJournalSequence { get; set; }
    }
}