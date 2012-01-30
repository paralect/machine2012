using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Serialization;

namespace Paralect.Machine
{
    public class MachineContext
    {
        private readonly MessageFactory _messageFactory;
        private readonly IdentityFactory _identityFactory;
        private readonly ProtobufSerializer _serializer;

        private EnvelopeSerializer _envelopeSerializer;

        public MachineContext(IEnumerable<Type> messageTypes, IEnumerable<Type> identityTypes  )
        {
            _messageFactory = new MessageFactory(messageTypes);
            _identityFactory = new IdentityFactory(identityTypes);

            _serializer = new ProtobufSerializer();
            _serializer.RegisterMessages(_messageFactory.MessageDefinitions);
            _serializer.RegisterIdentities(_identityFactory.IdentityDefinitions);

            _envelopeSerializer = new EnvelopeSerializer(_serializer, _messageFactory.TagToTypeResolver);
        }

        public static MachineContext Create(Action<MachineContextBuilder> action)
        {
            var builder = new MachineContextBuilder();
            action(builder);
            return builder.Build();
        }

        public Envelope CreateEnvelope(Action<EnvelopeBuilder> action)
        {
            var envelope = new EnvelopeBuilder(_messageFactory.TypeToTagResolver);
            action(envelope);
            return envelope.Build();
        }

        public BinaryEnvelope CreateBinaryEnvelope(Action<EnvelopeBuilder> action)
        {
            var envelope = new EnvelopeBuilder(_messageFactory.TypeToTagResolver);
            action(envelope);
            return envelope.BuildAndSerialize(_envelopeSerializer);
        }

    }

    public class MachineContextBuilder
    {
        public List<Type> _messageTypes = new List<Type>();
        public List<Type> _identityTypes = new List<Type>();


        public MachineContextBuilder RegisterMessages(params Type[] messageTypes)
        {
            _messageTypes.AddRange(messageTypes);
            return this;
        }

        public MachineContextBuilder RegisterIdentities(params Type[] identityTypes)
        {
            _identityTypes.AddRange(identityTypes);
            return this;
        }

        public MachineContext Build()
        {
            return new MachineContext(_messageTypes, _identityTypes);
        }
    }
}