using System;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using ProtoBuf;

namespace Paralect.Machine.Mongo.Tests.Messages
{
    [ProtoContract, Identity("{803b41b9-b566-4baf-a5ea-9744959fbac7}")]
    public class EnvelopeSerializer_Id : StringId { }

    [ProtoContract, Message("{74467730-33c0-418a-bd83-963258ce6fa9}")]
    public class EnvelopeSerializer_Event : IEvent<EnvelopeSerializer_Id>
    {
        [ProtoMember(1)]
        public String Title { get; set; }

        [ProtoMember(2)]
        public Double Rate { get; set; }
    }

    [ProtoContract, Message("{f55856e9-66b3-4fd4-9f6a-de9c2606a692}")]
    public class EnvelopeSerializer_Child_Event : EnvelopeSerializer_Event
    {
        [ProtoMember(1)]
        public String Child { get; set; }
    }
}