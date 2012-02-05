using Paralect.Machine.Identities;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    public class StateMetadata : IStateMetadata
    {
        [ProtoMember(1)]
        public int Version { get; set; }

        [ProtoMember(2)]
        public IIdentity ProcessId { get; set; }
    }
}