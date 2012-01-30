using System;
using Paralect.Machine.Messages;
using ProtoBuf;

namespace Paralect.Machine.Journals
{
    [ProtoContract, Message("{5f8cab30-9789-47f7-a666-90605d19c173}")]
    public class MessagesJournaledSuccessfully : IEvent
    {
        [ProtoMember(1)]
        public Int64 Sequence { get; set; }
    }
}