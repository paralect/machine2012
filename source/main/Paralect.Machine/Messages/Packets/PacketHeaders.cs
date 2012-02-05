using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    public class PacketHeaders : IPacketHeaders
    {
        [ProtoMember(1)]
        public ContentType ContentType { get; set; }
    }
}