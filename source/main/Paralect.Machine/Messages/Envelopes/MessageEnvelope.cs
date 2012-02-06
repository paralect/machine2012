using System;

namespace Paralect.Machine.Messages
{
    /// <summary>
    /// Message Envelope represents a wrapper around IMessage and IMessageMetadata.
    /// 
    /// Message Envelope knows how to serialize/deserialize yourself. This makes possible, for example, to deserialize only 
    /// message's metadata, without deserializing message. 
    /// 
    /// There are two ways to create Message Envelope:
    ///   1) By calling first constructor and passing message and metadata in the binary form.
    ///   2) By calling second constructor and passing IMessage and IMessageMetadata)
    /// </summary>
    public class MessageEnvelope : IMessageEnvelope
    {
        /// <summary>
        /// Packet serializer that responsible for message and metadata serialization/deserialization
        /// </summary>
        private readonly PacketSerializer _serializer;

        /// <summary>
        /// Message metadata in binary form
        /// </summary>
        private byte[] _metadataBinary;

        /// <summary>
        /// Message in binary form
        /// </summary>
        private byte[] _messageBinary;

        /// <summary>
        /// Message metadata contains additional "out-of-band" information about message
        /// </summary>
        private IMessageMetadata _metadata;

        /// <summary>
        /// Message
        /// </summary>
        private IMessage _message;

        /// <summary>
        /// Returns message metadata (deserialized, if needed).
        /// If metadata are available only in binary form - metadata will be deserialized and cached automatically. 
        /// Subsequent calls will use value from cache.
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
        /// Returns message (deserialized, if needed).
        /// If message are available only in binary form - message will be deserialized and cached automatically. 
        /// To deserialize message we need at least Message Tag that available in message metadata. That is why 
        /// by calling this method you'll actually fully deserialize envelope - metadata and message.
        /// 
        /// Subsequent calls will use value from cache.
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

        public MessageEnvelope(PacketSerializer packerSerializer, byte[] message, byte[] metadata)
        {
            
        }
    }
}