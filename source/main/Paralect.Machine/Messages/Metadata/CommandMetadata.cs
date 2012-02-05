using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    public class CommandMetadata : MessageMetadata, ICommandMetadata
    {
         
    }
}