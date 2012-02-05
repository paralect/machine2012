namespace Paralect.Machine.Messages
{
    public class MessageEnvelopeBinary : IMessageEnvelopeBinary
    {
        private readonly byte[] _metadataBinary;
        private readonly byte[] _messageBinary;

        public MessageEnvelopeBinary(byte[] messageBinary, byte[] metadataBinary)
        {
            _metadataBinary = metadataBinary;
            _messageBinary = messageBinary;
        }

        public byte[] GetMetadataBinary()
        {
            return _metadataBinary;
        }

        public byte[] GetMessageBinary()
        {
            return _messageBinary;
        }
    }
}