using System;
using ProtoBuf;

namespace Paralect.Machine.Transitions
{
    [ProtoContract]
    public class TransitionEnvelopeDataHeader
    {
        [ProtoMember(1)]
        public Guid[] MessageTags { get; set; }

        public TransitionEnvelopeDataHeader()
        {
        }

        public TransitionEnvelopeDataHeader(Guid[] messageTags)
        {
            MessageTags = messageTags;
        }
    }
}