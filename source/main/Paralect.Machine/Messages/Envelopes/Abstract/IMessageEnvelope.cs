namespace Paralect.Machine.Messages
{
    public interface IMessageEnvelope
    {
        IMessageMetadata Metadata { get; }
        IMessage Message { get; }
    }
}