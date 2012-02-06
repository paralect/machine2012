using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;
using Paralect.Machine.Nodes;
using Paralect.Machine.Serialization;
using Paralect.Machine.Utilities;
using ZMQ;
using Socket = Paralect.Machine.Sockets.Socket;

namespace Paralect.Machine.Routers
{
    public class RouterNode : INode
    {
        private readonly MachineContext _context;
        private readonly String _routerRepAddress;
        private readonly String _routerPubAddress;
        private readonly String _domainReqAddress;
        private readonly IJournalStorage _storage;

        public RouterNode(MachineContext context, String routerRepAddress, String routerPubAddress, String domainReqAddress, IJournalStorage storage)
        {
            _context = context;
            _routerRepAddress = routerRepAddress;
            _routerPubAddress = routerPubAddress;
            _domainReqAddress = domainReqAddress;
            _storage = storage;
        }

        public void Init()
        {
            
        }

        public void Run(CancellationToken token)
        {
            using (Socket routerRepSocket = _context.CreateSocket(SocketType.PULL))
            using (Socket routerPubSocket = _context.CreateSocket(SocketType.PUB))
            using (Socket domainReqSocket = _context.CreateSocket(SocketType.REQ))
            {
                // Bind and connect to sockets
                routerRepSocket.Bind(_routerRepAddress);
                routerPubSocket.Bind(_routerPubAddress);
                //domainReqSocket.EstablishConnect(_domainReqAddress, token);

                // Process while canellation not requested
                while (!token.IsCancellationRequested)
                {
                    // Waits for binary envelopes (with timeout)
                    var packet = routerRepSocket.RecvPacket(200);
                    if (packet == null) continue;

                    // Ignore packets without messages
                    if (packet.GetHeaders().ContentType != ContentType.Messages)
                        continue;

                    // Journal all messages
                    var seq = JournalPacket(packet);

                    // Publish all messages
                    PublishMessages(routerPubSocket, packet, seq);
                }
            }
        }

        private Int64 JournalPacket(IPacket packet)
        {
            // Journal all messages
            var seq = _storage.Save(packet.GetEnvelopesCloned());
            return seq;
        }

        private void PublishMessages(Socket routerPubSocket, IPacket packet, Int64 seq)
        {
            var clonedEnvelopes = packet.GetEnvelopesCloned();

            for (int i = 0; i < clonedEnvelopes.Count; i++)
            {
                var envelope = clonedEnvelopes[i];
                
                var metadata = envelope.GetMetadata();
                // Set sequence for each message
                metadata.JournalStreamSequence = seq - clonedEnvelopes.Count + i + 1;

                var newPacket = _context.CreatePacket(b => b
                    .AddMessage(envelope.GetMessageBinary(), metadata)
                );

                routerPubSocket.SendPacket(newPacket);
            }
        }

        public void Dispose()
        {
            
        }
    }
}