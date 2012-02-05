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

        public IEnumerable<byte[]> Serialize(IEnumerable<IMessageEnvelope> envelopes)
        {
            var memory = new MemoryStream();

            foreach (var item in envelopes)
            {
                var binaryMessageEnvelope = WriteMessageEnvelope(item);

                foreach (var bytese in binaryMessageEnvelope)
                    yield return bytese;
            }
        }

        private IEnumerable<byte[]> WriteMessageEnvelope(IMessageEnvelope messageEnvelope)
        {
            using (var headerMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(headerMemory, messageEnvelope.GetMetadata(), messageEnvelope.GetMetadata().GetType(), PrefixStyle.Base128, 0);
                yield return headerMemory.ToArray();
            }

            using (var messageMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(messageMemory, messageEnvelope.GetMessage(), messageEnvelope.GetMessage().GetType(), PrefixStyle.Base128, 0);
                yield return messageMemory.ToArray();
            }
        }

        public IEnumerable<byte[]> Serialize(IMessageEnvelope messageEnvelope)
        {
            var parts = WriteMessageEnvelope(messageEnvelope);

            foreach (var part in parts)
                yield return part;
        }

        public IEnumerable<IMessageEnvelope> Deserialize(IEnumerable<byte[]> parts)
        {
            var enumerator = parts.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var metadata = enumerator.Current;
                enumerator.MoveNext();
                var message = enumerator.Current;
                yield return ReadMessageEnvelope(metadata, message);
            }
        }

        private IMessageEnvelope ReadMessageEnvelope(byte[] metadataBytes, byte[] messageBytes)
        {
            IMessageMetadata messageMetadata = null;
            IMessage message = null;

            using(var headerMemory = new MemoryStream(metadataBytes))
            {
                messageMetadata = (IMessageMetadata) _serializer.Model.DeserializeWithLengthPrefix(headerMemory, null, typeof(MessageMetadata), PrefixStyle.Base128, 0, null);    
            }

            var messageType = _tagToTypeResolver(messageMetadata.MessageTag);

            using (var messageMemory = new MemoryStream(messageBytes))
            {
                message = (IMessage)_serializer.Model.DeserializeWithLengthPrefix(messageMemory, null, messageType, PrefixStyle.Base128, 0, null);
            }

            return EnvelopeFactory.CreateEnvelope(message, messageMetadata);
        }

        public IMessageEnvelope DeserializeMessageEnvelope(IEnumerable<byte[]> binaryMessageEnvelope)
        {
            var metadata = binaryMessageEnvelope.First();
            var message = binaryMessageEnvelope.ElementAt(1);
            return ReadMessageEnvelope(metadata, message);
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