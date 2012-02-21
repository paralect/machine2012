using System;
using System.Collections.Generic;
using System.Threading;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Nodes;
using Paralect.Machine.Routers;
using Paralect.Machine.Serialization;
using Paralect.Machine.Sockets;

namespace Paralect.Machine
{
    public class MachineContext
    {
        private readonly MessageFactory _messageFactory;
        private readonly IdentityFactory _identityFactory;
        private readonly RouterFactory _routerFactory;

        private readonly ProtobufSerializer _serializer;

        private readonly PacketSerializer _packetSerializer;

        private readonly ZMQ.Context _zeromqContext;

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

        public PacketSerializer PacketSerializer
        {
            get { return _packetSerializer; }
        }

        public RouterFactory RouterFactory
        {
            get { return _routerFactory; }
        }

        public ZMQ.Context ZmqContext
        {
            get { return _zeromqContext; }
        }

        public PacketBuilder CreatePacketBuilder()
        {
            return new PacketBuilder(_messageFactory.TypeToTagResolver, _packetSerializer);
        }

        public MachineContext(IEnumerable<Type> messageTypes, IEnumerable<Type> identityTypes, IDictionary<String, IRouter> routers)
        {
            _messageFactory = new MessageFactory(messageTypes);
            _identityFactory = new IdentityFactory(identityTypes);

            _serializer = new ProtobufSerializer();
            _serializer.RegisterMessages(_messageFactory.MessageDefinitions);
            _serializer.RegisterIdentities(_identityFactory.IdentityDefinitions);

            _packetSerializer = new PacketSerializer(_serializer, _messageFactory.TagToTypeResolver);

            _routerFactory = new RouterFactory(routers);

            _zeromqContext = new ZMQ.Context(2);
        }

        public static MachineContext Create(Action<MachineContextBuilder> action)
        {
            var builder = new MachineContextBuilder();
            action(builder);
            return builder.Build();
        }

        public IPacket CreatePacket(Action<PacketBuilder> action)
        {
            var envelope = new PacketBuilder(_messageFactory.TypeToTagResolver, _packetSerializer);
            action(envelope);
            return envelope.Build();
        } 

        public IPacket CreatePacket(IList<IPacketMessageEnvelope> envelopes, IPacketHeaders headers = null)
        {
            return new Packet(_packetSerializer, headers ?? new PacketHeaders(), envelopes);
        }

        public IPacket CreatePacket(IList<byte[]> parts)
        {
            return new Packet(_packetSerializer, parts);
        } 

        public IPacket CreatePacket(IMessage message)
        {
            var envelope = new PacketBuilder(_messageFactory.TypeToTagResolver, _packetSerializer);
            envelope.AddMessage(message);
            return envelope.Build();
        } 

        public IPacket CreatePacket(IMessage message, IMessageMetadata metadata)
        {
            var envelope = new PacketBuilder(_messageFactory.TypeToTagResolver, _packetSerializer);
            envelope.AddMessage(message, metadata);
            return envelope.Build();
        } 

        public void RunHost(Action<MachineHostBuilder> action)
        {
            var builder = new MachineHostBuilder();
            action(builder);
            var host = builder.Build();

            using (var token = new CancellationTokenSource())
            {
                var task = host.Start(token.Token, builder._timeout);

                builder._action(token.Token);

                task.Wait();
//                token.Cancel();
            }
        }

        public Socket CreateSocket(ZMQ.SocketType socketType)
        {
            var zmqsocket = _zeromqContext.Socket(socketType);
            var socket = new Socket(this, zmqsocket);
            return socket;
        }
    }

    public class MachineContextBuilder
    {
        public List<Type> _messageTypes = new List<Type>();
        public List<Type> _identityTypes = new List<Type>();
        public Dictionary<String, IRouter> _routers = new Dictionary<string, IRouter>();


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

        public MachineContextBuilder AddRouter(String name, IRouter router)
        {
            _routers[name] = router;
            return this;
        }

        public MachineContext Build()
        {
            return new MachineContext(_messageTypes, _identityTypes, _routers);
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