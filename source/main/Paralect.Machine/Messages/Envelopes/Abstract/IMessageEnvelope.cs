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
}