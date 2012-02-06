using System;

namespace Paralect.Machine.Messages
{
    public class MessageEnvelope : IMessageEnvelope
    {
        private byte[] _metadataBinary;
        private byte[] _messageBinary;

        private readonly PacketSerializer _serializer;
        private IMessageMetadata _metadata;
        private IMessage _message;

        /// <summary>
        /// Message tag and key-value information
        /// </summary>
        public IMessageMetadata GetMetadata()
        {
            if (_metadata == null)
            {
                _metadata = GetMetadataCopy();
                _metadataBinary = null;
            }

            return _metadata;
        }

        public IMessageMetadata GetMetadataCopy()
        {
            return _serializer.DeserializeMessageMetadata(_metadataBinary);
        }

        /// <summary>
        /// Actual single message in the envelope
        /// </summary>
        public IMessage GetMessage()
        {
            if (_message == null)
            {
                _message = GetMessageCopy();
                _messageBinary = null;
            }

            return _message;
        }

        private IMessage GetMessageCopy()
        {
            var metadata = GetMetadata();
            return _serializer.DeserializeMessage(_messageBinary, metadata.MessageTag);
        }

        public byte[] GetMetadataBinary()
        {
            if (_metadataBinary == null)
                _metadataBinary = _serializer.SerializeMessageMetadata(_metadata);

            return _metadataBinary;
        }

        public byte[] GetMessageBinary()
        {
            if (_messageBinary == null)
                _messageBinary = _serializer.SerializeMessage(_message);

            return _messageBinary;
        }


        /// <summary>
        /// Constructs MessageEnvelope
        /// </summary>
        public MessageEnvelope(PacketSerializer packetSerializer, IMessageMetadata metadata, IMessage message)
        {
            if (packetSerializer == null) throw new ArgumentNullException("packetSerializer");
            if (metadata == null) throw new ArgumentNullException("metadata");
            if (message == null) throw new ArgumentNullException("message");

//            if (metadata.MessageTag == Guid.Empty)
//                throw new Exception("Message tag was not specified in message header");

            _serializer = packetSerializer;
            _metadata = metadata;
            _message = message;
        }
    }
}