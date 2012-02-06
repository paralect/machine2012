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
    ///   1) By calling "first" constructor and passing message and metadata in the binary form.
    ///   2) By calling "second" constructor and passing IMessage and IMessageMetadata
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
                _metadata = _serializer.DeserializeMessageMetadata(_metadataBinary);
                _metadataBinary = null;
            }

            return _metadata;
        }

        /// <summary>
        /// Returns message metadata in binary form.
        /// If metadata are not available in binary form - metadata will be serialized and cached automatically.
        /// 
        /// Subsequent calls will use value from cache.
        /// </summary>
        public byte[] GetMetadataBinary()
        {
            if (_metadataBinary == null)
                _metadataBinary = _serializer.SerializeMessageMetadata(_metadata);

            return _metadataBinary;
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
                var metadata = GetMetadata();
                _message = _serializer.DeserializeMessage(_messageBinary, metadata.MessageTag);
                _messageBinary = null;
            }

            return _message;
        }

        /// <summary>
        /// Returns message data in binary form.
        /// If message are not available in binary form - message will be serialized and cached automatically.
        /// 
        /// Subsequent calls will use value from cache.
        /// </summary>
        public byte[] GetMessageBinary()
        {
            if (_messageBinary == null)
                _messageBinary = _serializer.SerializeMessage(_message);

            return _messageBinary;
        }


        /// <summary>
        /// Creates MessageEnvelope with specified message and metadata
        /// </summary>
        public MessageEnvelope(PacketSerializer packetSerializer, IMessage message, IMessageMetadata metadata)
        {
            if (packetSerializer == null) throw new ArgumentNullException("packetSerializer");
            if (metadata == null) throw new ArgumentNullException("metadata");
            if (message == null) throw new ArgumentNullException("message");

            _serializer = packetSerializer;
            _metadata = metadata;
            _message = message;
        }

        /// <summary>
        /// Creates MessageEnvelope from message and metadata in binary form
        /// </summary>
        public MessageEnvelope(PacketSerializer packetSerializer, byte[] message, byte[] metadata)
        {
            if (packetSerializer == null) throw new ArgumentNullException("packetSerializer");
            if (message == null) throw new ArgumentNullException("message");
            if (metadata == null) throw new ArgumentNullException("metadata");

            _serializer = packetSerializer;
            _metadataBinary = metadata;
            _messageBinary = message;
        }
    }
}