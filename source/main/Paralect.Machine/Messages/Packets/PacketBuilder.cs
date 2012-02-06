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
            var envelope = EnvelopeFactory.CreateEnvelope(_serializer, message, metadata);
            envelope.GetMetadata().MessageTag = _typeToTagResolver(message.GetType());

            _envelopes.Add(envelope);
            
            return this;
        }

        /// <summary>
        /// Dangerous method for adding new message, because we unable to check correct state of MessageTag.
        /// To check this we should deserialize message. For performance reason we are avoiding this. 
        /// Here we just checking that metadata has MessageTag specified.
        /// </summary>
        public PacketBuilder AddMessage(byte[] message, IMessageMetadata metadata)
        {
            var envelope = EnvelopeFactory.CreateEnvelope(_serializer, message, metadata);

            if (metadata.MessageTag == default(Guid))
                throw new Exception("Message tag not specified");

            _envelopes.Add(envelope);

            return this;            
        }

        public PacketBuilder AddMessage(IMessage message)
        {
            var envelope = EnvelopeFactory.CreateEnvelope(_serializer, message);
            envelope.GetMetadata().MessageTag = _typeToTagResolver(message.GetType());;

            _envelopes.Add(envelope);
            
            return this;
        }

        public IPacket Build()
        {
            return new Packet(_serializer, new PacketHeaders(), _envelopes);
        }
    }
}