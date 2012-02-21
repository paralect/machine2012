namespace Paralect.Machine.Messages
{
    public interface IMessageEnvelope
    {
        IMessageMetadata GetMetadata();
        IMessage GetMessage();
        IMessageEnvelope CloneEnvelope();

        byte[] GetMetadataBinary();
        byte[] GetMessageBinary();
    }

    public interface IMessageEnvelope2
    {
        IMessage Message { get; }

        IMessageMetadata Metadata { get; }
    }

    public interface IPackedMessageEnvelope : IMessageEnvelope2
    {
        IPackedMessageEnvelope CloneEnvelope();

        byte[] MetadataBinary { get; }
        byte[] MessageBinary { get; }
    }
}