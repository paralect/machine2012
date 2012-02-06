using System;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    public class PacketBuilder
    {
        private readonly Func<Type, Guid> _typeToTagResolver;
        private readonly PacketSerializer _serializer;

        private readonly List<IMessageEnvelope> _envelopes = new List<IMessageEnvelope>();

        public PacketBuilder(Func<Type, Guid> typeToTagResolver, PacketSerializer serializer)
        {
            _typeToTagResolver = typeToTagResolver;
            _serializer = serializer;
        }

        public PacketBuilder AddMessage(IMessage message, IMessageMetadata metadata)
        {
            //var messageTag = _typeToTagResolver(message.GetType());

            var envelope = EnvelopeFactory.CreateEnvelope(_serializer, message, metadata);
            _envelopes.Add(envelope);
            
            return this;
        }

        public PacketBuilder AddMessage(IMessage message)
        {
            var messageTag = _typeToTagResolver(message.GetType());

            var envelope = EnvelopeFactory.CreateEnvelope(_serializer, message);
            envelope.GetMetadata().MessageTag = messageTag;

            _envelopes.Add(envelope);
            
            return this;
        }

        public IPacket Build()
        {
            return new Packet(_serializer, new PacketHeaders(), _envelopes);
        }
    }
}