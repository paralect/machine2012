using System;
using System.IO;
using Paralect.Machine.Serialization;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    public class EnvelopeSerializer
    {
        private readonly ProtobufSerializer _serializer;
        private readonly Func<Guid, Type> _tagToTypeResolver;

        public EnvelopeSerializer(ProtobufSerializer serializer, Func<Guid, Type> tagToTypeResolver)
        {
            _serializer = serializer;
            _tagToTypeResolver = tagToTypeResolver;
        }

        public BinaryEnvelope Serialize(Envelope envelope)
        {
            var binaryEnvelope = new BinaryEnvelope();
            var memory = new MemoryStream();
            _serializer.Model.SerializeWithLengthPrefix(memory, envelope.Header, typeof(EnvelopeHeader), PrefixStyle.Base128, 0);
            binaryEnvelope.Header = memory.ToArray();

            foreach (var item in envelope.Items)
            {
                var binaryMessageEnvelope = WriteMessageEnvelope(item);
                binaryEnvelope.AddBinaryMessageEnvelope(binaryMessageEnvelope);
            }

            return binaryEnvelope;
        }

        private BinaryMessageEnvelope WriteMessageEnvelope(MessageEnvelope messageEnvelope)
        {
            var binaryMessageEnvelope = new BinaryMessageEnvelope();

            using (var headerMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(headerMemory, messageEnvelope.Header, typeof(MessageHeader), PrefixStyle.Base128, 0);
                binaryMessageEnvelope.Header = headerMemory.ToArray();
            }

            using (var messageMemory = new MemoryStream())
            {
                _serializer.Model.SerializeWithLengthPrefix(messageMemory, messageEnvelope.Message, messageEnvelope.Message.GetType(), PrefixStyle.Base128, 0);
                binaryMessageEnvelope.Message = messageMemory.ToArray();
            }

            return binaryMessageEnvelope;
        }

        public BinaryMessageEnvelope Serialize(MessageEnvelope messageEnvelope)
        {
            return WriteMessageEnvelope(messageEnvelope);
        }

        public Envelope Deserialize(BinaryEnvelope binaryEnvelope)
        {
            var envelope = new Envelope();


            using (var memory = new MemoryStream(binaryEnvelope.Header))
            {
                envelope.Header = (EnvelopeHeader)_serializer.Model.DeserializeWithLengthPrefix(memory, null, typeof(EnvelopeHeader), PrefixStyle.Base128, 0, null);
            }
            
            foreach (var binaryMessageEnvelope in binaryEnvelope.MessageEnvelopes)
            {
                var item = ReadMessageEnvelope(binaryMessageEnvelope);
                envelope.AddItem(item);
            }

            return envelope;
        }

        private MessageEnvelope ReadMessageEnvelope(BinaryMessageEnvelope binaryMessageEnvelope)
        {
            MessageHeader messageHeader = null;
            IMessage message = null;

            using(var headerMemory = new MemoryStream(binaryMessageEnvelope.Header))
            {
                messageHeader = (MessageHeader)_serializer.Model.DeserializeWithLengthPrefix(headerMemory, null, typeof(MessageHeader), PrefixStyle.Base128, 0, null);    
            }

            var messageType = _tagToTypeResolver(messageHeader.MessageTag);

            using (var messageMemory = new MemoryStream(binaryMessageEnvelope.Message))
            {
                message = (IMessage)_serializer.Model.DeserializeWithLengthPrefix(messageMemory, null, messageType, PrefixStyle.Base128, 0, null);
            }
            
            return new MessageEnvelope(messageHeader, message);
        }

        public MessageEnvelope DeserializeMessageEnvelope(BinaryMessageEnvelope binaryMessageEnvelope)
        {
            return ReadMessageEnvelope(binaryMessageEnvelope);
        }
    }
}