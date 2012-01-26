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

        public byte[] Serialize(Envelope transition)
        {
            var memory = new MemoryStream();
            _serializer.Model.SerializeWithLengthPrefix(memory, transition.Header, typeof(EnvelopeHeader), PrefixStyle.Base128, 0);

            foreach (var item in transition.Items)
            {
                WriteMessageEnvelope(memory, item);
            }

            // TODO: ToArray() is inefficient because of copy
            return memory.ToArray();
        }

        private void WriteMessageEnvelope(MemoryStream memory, MessageEnvelope messageEnvelope)
        {
            _serializer.Model.SerializeWithLengthPrefix(memory, messageEnvelope.Header, typeof(MessageHeader), PrefixStyle.Base128, 0);
            _serializer.Model.SerializeWithLengthPrefix(memory, messageEnvelope.Message, messageEnvelope.Message.GetType(), PrefixStyle.Base128, 0);
        }

        public byte[] Serialize(MessageEnvelope messageEnvelope)
        {
            var memory = new MemoryStream();
            WriteMessageEnvelope(memory, messageEnvelope);
            return memory.ToArray();
        }

        public Envelope Deserialize(byte[] bytes)
        {
            var memory = new MemoryStream(bytes);

            var envelope = new Envelope();

            envelope.Header = (EnvelopeHeader) _serializer.Model.DeserializeWithLengthPrefix(memory, null, typeof(EnvelopeHeader), PrefixStyle.Base128, 0, null);
            while (true)
            {
                var item = ReadMessageEnvelope(memory, bytes);

                if (item == null) // we are reached the end
                    break;

                envelope.AddItem(item);
            }

            return envelope;
        }

        private MessageEnvelope ReadMessageEnvelope(MemoryStream memory, byte[] bytes)
        {
            var messageHeader = (MessageHeader) _serializer.Model.DeserializeWithLengthPrefix(memory, null, typeof(MessageHeader), PrefixStyle.Base128, 0, null);

            if (messageHeader == null) // we are reached the end
                return null;

            var messageType = _tagToTypeResolver(messageHeader.MessageTag);
            var message = (IMessage) _serializer.Model.DeserializeWithLengthPrefix(memory, null, messageType, PrefixStyle.Base128, 0, null);

            return new MessageEnvelope(messageHeader, message);
        }

        public MessageEnvelope DeserializeMessageEnvelope(Func<Guid, Type> tagToTypeResolver , ProtobufSerializer serializer, byte[] bytes)
        {
            var memory = new MemoryStream(bytes);
            var item = ReadMessageEnvelope(memory, bytes);
            return item;
        }
    }
}