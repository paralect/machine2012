using System;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    public class PacketBuilder
    {
        private readonly Func<Type, Guid> _typeToTagResolver;

        private readonly List<IMessageEnvelope> _envelopes = new List<IMessageEnvelope>();

        public PacketBuilder(Func<Type, Guid> typeToTagResolver)
        {
            _typeToTagResolver = typeToTagResolver;
        }

        public PacketBuilder AddMessage(IMessage message, IMessageMetadata metadata)
        {
            //var messageTag = _typeToTagResolver(message.GetType());

            var envelope = EnvelopeFactory.CreateEnvelope(message, metadata);
            _envelopes.Add(envelope);
            
            return this;
        }

        public PacketBuilder AddMessage(IMessage message)
        {
            var messageTag = _typeToTagResolver(message.GetType());

            var envelope = EnvelopeFactory.CreateEnvelope(message);
            envelope.GetMetadata().MessageTag = messageTag;

            _envelopes.Add(envelope);
            
            return this;
        }

        public IPacket Build(PacketSerializer serializer)
        {
            return new Packet(serializer, new PacketHeaders(), _envelopes);
        }
    }
}