namespace Paralect.Machine.Messages
{
    public interface IMessageEnvelope
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