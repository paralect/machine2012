using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Paralect.Machine.Serialization;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    public class PacketSerializer
    {
        private readonly ProtobufSerializer _serializer;
        private readonly Func<Guid, Type> _tagToTypeResolver;

        public PacketSerializer(ProtobufSerializer serializer, Func<Guid, Type> tagToTypeResolver)
        {
            _serializer = serializer;
            _tagToTypeResolver = tagToTypeResolver;
        }

        #region Message serialization/deserialization

        public byte[] SerializeMessage(IMessage messageEnvelope)
        {
            byte[] messageBinary;

            using (var messageMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(messageMemory, messageEnvelope, messageEnvelope.GetType(), PrefixStyle.Base128, 0);
                messageBinary = messageMemory.ToArray();
            }

            return messageBinary;
        }

        public IMessage DeserializeMessage(byte[] binary, Guid messageTag)
        {
            IMessage message = null;

            var messageType = _tagToTypeResolver(messageTag);

            using (var messageMemory = new MemoryStream(binary))
            {
                message = (IMessage)_serializer.Model.DeserializeWithLengthPrefix(messageMemory, null, messageType, PrefixStyle.Base128, 0, null);
            }

            return message;
        }

        #endregion

        #region Metadata serialization/deserialization

        public byte[] SerializeMessageMetadata(IMessageMetadata metadata)
        {
            byte[] metadataBinary;

            using (var headerMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(headerMemory, metadata, metadata.GetType(), PrefixStyle.Base128, 0);
                metadataBinary = headerMemory.ToArray();
            }

            return metadataBinary;
        }

        public IMessageMetadata DeserializeMessageMetadata(byte[] binary)
        {
            IMessageMetadata messageMetadata = null;

            using(var headerMemory = new MemoryStream(binary))
            {
                messageMetadata = (IMessageMetadata) _serializer.Model.DeserializeWithLengthPrefix(headerMemory, null, typeof(MessageMetadata), PrefixStyle.Base128, 0, null);    
            }

            return messageMetadata;
        }

        #endregion

        #region Packet serialization/deserialization

        public byte[] SerializePacketHeaders(IPacketHeaders headers)
        {
            using (var messageMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(messageMemory, headers, headers.GetType(), PrefixStyle.Base128, 0);
                return messageMemory.ToArray();
            }
        }

        public IPacketHeaders DeserializePacketHeaders(byte[] bytes)
        {
            using (var messageMemory = new MemoryStream(bytes))
            {
                return (IPacketHeaders) _serializer.Model.DeserializeWithLengthPrefix(messageMemory, null, typeof(PacketHeaders), PrefixStyle.Base128, 0, null);
            }            
        }

        #endregion
    }
}