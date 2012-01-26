using System;
using Paralect.Machine.Serialization;

namespace Paralect.Machine.Messages
{
    public class EnvelopeBuilder
    {
        private readonly Func<Type, Guid> _typeToTagResolver;
        private readonly Envelope _envelope;

        public EnvelopeBuilder(Func<Type, Guid> typeToTagResolver)
        {
            _typeToTagResolver = typeToTagResolver;
            _envelope = new Envelope();
        }

        public EnvelopeBuilder AddMessage(IMessage message)
        {
            var messageTag = _typeToTagResolver(message.GetType());
            var messageHeader = new MessageHeader(messageTag);
            
            messageHeader.AddMetadata("Description", message.ToString());

            var envelopeItem = new MessageEnvelope(messageHeader, message);
            _envelope.AddItem(envelopeItem);
            
            return this;
        }

        public EnvelopeBuilder AddMessageEnvelope(MessageEnvelope messageEnvelope)
        {
            _envelope.AddItem(messageEnvelope);
            return this;
        }

        public EnvelopeBuilder AddEnvelopeMetadata(String key, String value)
        {
            _envelope.Header.Metadata.Add(key, value);
            return this;
        }

        public Envelope Build()
        {
            return _envelope;
        }

        public byte[] BuildAndSerialize(EnvelopeSerializer serializer)
        {
            var envelope = Build();
            var answerBytes = serializer.Serialize(envelope);
            return answerBytes;
        }
    }
}