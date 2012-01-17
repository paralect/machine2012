using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface IMessage
    {
        IMessageMetadata Metadata { get; set; }
    }
}