using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface IMessage
    {
        IMessageMetadata Metadata { get; set; }
    }

    public interface IMessage<TMessageId> : IMessage 
        where TMessageId : IIdentity
    {
        new IMessageMetadata<TMessageId> Metadata { get; set; }
    }
}