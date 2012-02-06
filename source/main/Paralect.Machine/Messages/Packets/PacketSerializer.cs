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

        public IList<IMessageEnvelopeBinary> Serialize(IEnumerable<IMessageEnvelope> envelopes)
        {
            var list = new List<IMessageEnvelopeBinary>();

            foreach (var item in envelopes)
            {
                var binary = SerializeMessageEnvelope(item);
                list.Add(binary);
            }

            return list.AsReadOnly();
        }

        public IMessageEnvelopeBinary SerializeMessageEnvelope(IMessageEnvelope messageEnvelope)
        {
            byte[] metadataBinary;
            byte[] messageBinary;

            using (var headerMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(headerMemory, messageEnvelope.GetMetadata(), messageEnvelope.GetMetadata().GetType(), PrefixStyle.Base128, 0);
                metadataBinary = headerMemory.ToArray();
            }

            using (var messageMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(messageMemory, messageEnvelope.GetMessage(), messageEnvelope.GetMessage().GetType(), PrefixStyle.Base128, 0);
                messageBinary = messageMemory.ToArray();
            }

            return new MessageEnvelopeBinary(messageBinary, metadataBinary);
        }        
        
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

        public IList<IMessageEnvelope> Deserialize(IEnumerable<IMessageEnvelopeBinary> binaries)
        {
            var list = new List<IMessageEnvelope>();

            foreach (var binary in binaries)
            {
                var envelope = DeserializeMessageEnvelope(binary);
                list.Add(envelope);
            }

            return list.AsReadOnly();
        }

        public IMessageEnvelope DeserializeMessageEnvelope(IMessageEnvelopeBinary binary)
        {
            IMessageMetadata messageMetadata = null;
            IMessage message = null;

            using(var headerMemory = new MemoryStream(binary.GetMetadataBinary()))
            {
                messageMetadata = (IMessageMetadata) _serializer.Model.DeserializeWithLengthPrefix(headerMemory, null, typeof(MessageMetadata), PrefixStyle.Base128, 0, null);    
            }

            var messageType = _tagToTypeResolver(messageMetadata.MessageTag);

            using (var messageMemory = new MemoryStream(binary.GetMessageBinary()))
            {
                message = (IMessage)_serializer.Model.DeserializeWithLengthPrefix(messageMemory, null, messageType, PrefixStyle.Base128, 0, null);
            }

            return EnvelopeFactory.CreateEnvelope(this, message, messageMetadata);
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

        public IMessageMetadata DeserializeMessageMetadata(byte[] binary)
        {
            IMessageMetadata messageMetadata = null;

            using(var headerMemory = new MemoryStream(binary))
            {
                messageMetadata = (IMessageMetadata) _serializer.Model.DeserializeWithLengthPrefix(headerMemory, null, typeof(MessageMetadata), PrefixStyle.Base128, 0, null);    
            }

            return messageMetadata;
        }


        public byte[] SerializeHeaders(IPacketHeaders headers)
        {
            using (var messageMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(messageMemory, headers, headers.GetType(), PrefixStyle.Base128, 0);
                return messageMemory.ToArray();
            }
        }

        public IPacketHeaders DeserializeHeaders(byte[] bytes)
        {
            using (var messageMemory = new MemoryStream(bytes))
            {
                return (IPacketHeaders) _serializer.Model.DeserializeWithLengthPrefix(messageMemory, null, typeof(PacketHeaders), PrefixStyle.Base128, 0, null);
            }            
        }
    }
}