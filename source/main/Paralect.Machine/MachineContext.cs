using System;
using System.Collections.Generic;
using System.Threading;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Nodes;
using Paralect.Machine.Packets;
using Paralect.Machine.Routers;
using Paralect.Machine.Serialization;
using ZMQ;

namespace Paralect.Machine
{
    public class MachineContext
    {
        private readonly MessageFactory _messageFactory;
        private readonly IdentityFactory _identityFactory;
        private readonly ProtobufSerializer _serializer;

        private readonly EnvelopeSerializer _envelopeSerializer;
        private readonly Packets.PacketPartsSerializer _partsSerializer;

        private readonly Context _zeromqContext;

        public MessageFactory MessageFactory
        {
            get { return _messageFactory; }
        }

        public IdentityFactory IdentityFactory
        {
            get { return _identityFactory; }
        }

        public ProtobufSerializer Serializer
        {
            get { return _serializer; }
        }

        public EnvelopeSerializer EnvelopeSerializer
        {
            get { return _envelopeSerializer; }
        }

        public Context ZeromqContext
        {
            get { return _zeromqContext; }
        }

        public MachineContext(IEnumerable<Type> messageTypes, IEnumerable<Type> identityTypes  )
        {
            _messageFactory = new MessageFactory(messageTypes);
            _identityFactory = new IdentityFactory(identityTypes);

            _serializer = new ProtobufSerializer();
            _serializer.RegisterMessages(_messageFactory.MessageDefinitions);
            _serializer.RegisterIdentities(_identityFactory.IdentityDefinitions);

            _envelopeSerializer = new EnvelopeSerializer(_serializer, _messageFactory.TagToTypeResolver);
            _partsSerializer = new PacketPartsSerializer(_serializer, _messageFactory.TagToTypeResolver);

            _zeromqContext = new Context(2);
        }

        public static MachineContext Create(Action<MachineContextBuilder> action)
        {
            var builder = new MachineContextBuilder();
            action(builder);
            return builder.Build();
        }

        public IPacket CreatePacket(Action<PacketBuilder> action)
        {
            var envelope = new PacketBuilder(_messageFactory.TypeToTagResolver);
            action(envelope);
            return envelope.Build(_partsSerializer);
        } 

        public BinaryEnvelope CreateBinaryEnvelope(Action<EnvelopeBuilder> action)
        {
            var envelope = new EnvelopeBuilder(_messageFactory.TypeToTagResolver);
            action(envelope);
            return envelope.BuildAndSerialize(_envelopeSerializer);
        }        
        
        public BinaryEnvelope CreateBinaryEnvelope(params IMessage[] messages)
        {
            var envelope = new EnvelopeBuilder(_messageFactory.TypeToTagResolver);

            foreach (var message in messages)
            {
                envelope.AddMessage(message);
            }
            
            return envelope.BuildAndSerialize(_envelopeSerializer);
        }

        public void RunHost(Action<MachineHostBuilder> action)
        {
            var builder = new MachineHostBuilder();
            action(builder);
            var host = builder.Build();

            using (var token = new CancellationTokenSource())
            {
                var task = host.Start(token.Token);

                builder._action(token.Token);

                if (!task.Wait(builder._timeout))
                    Console.WriteLine("\r\nShutdowning host...");

                token.Cancel();
            }


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

    public class MachineHostBuilder
    {
        public List<INode> _nodex = new List<INode>();
        public Action<CancellationToken> _action;
        public Int32 _timeout = 50;


        public MachineHostBuilder AddNode(INode node)
        {
            _nodex.Add(node);
            return this;
        }

        public MachineHostBuilder Execute(Action<CancellationToken> action)
        {
            _action = action;
            return this;
        }

        public MachineHostBuilder SetTimeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }

        public Host Build()
        {
            return new Host(_nodex);
        }
    }
}