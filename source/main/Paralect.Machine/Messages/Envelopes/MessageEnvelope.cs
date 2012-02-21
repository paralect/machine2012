namespace Paralect.Machine.Messages
{
    public class MessageEnvelope : IMessageEnvelope
    {
        public IMessageMetadata Metadata { get; private set; }
        public IMessage Message { get; private set; }

        public MessageEnvelope(IMessageMetadata metadata, IMessage message)
        {
            Metadata = metadata;
            Message = message;
        }
    }
}