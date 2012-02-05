using System;
using System.Collections.Generic;
using Paralect.Machine.Envelopes;
using Paralect.Machine.Messages;
using Paralect.Machine.Metadata;

namespace Paralect.Machine.Packets
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

            var envelope = new Envelopes.MessageEnvelope(metadata, message);
            _envelopes.Add(envelope);
            
            return this;
        }

        public IPacket Build(PacketPartsSerializer serializer)
        {
            var parts = serializer.Serialize(_envelopes);
            return new Packet(serializer, new PacketHeaders(), parts);
        }
    }
}