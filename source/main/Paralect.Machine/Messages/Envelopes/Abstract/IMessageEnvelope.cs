namespace Paralect.Machine.Messages
{
    public interface IMessageEnvelope : IMessageEnvelopeBinary
    {
        IMessageMetadata GetMetadata();
        IMessage GetMessage();
    }

    public interface IMessageEnvelopeBinary
    {
        byte[] GetMetadataBinary();
        byte[] GetMessageBinary();
    }
}