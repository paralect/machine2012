namespace Paralect.Machine.Messages
{
    public class MessageEnvelope : IMessageEnvelope
    {
        public IMessageMetadata Metadata { get; private set; }
        public IMessage Message { get; private set; }

        public MessageEnvelope(IMessage message, IMessageMetadata metadata)
        {
            Metadata = metadata;
            Message = message;
        }
    }
}