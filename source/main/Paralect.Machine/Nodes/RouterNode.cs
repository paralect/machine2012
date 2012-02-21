using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;
using ZMQ;
using Socket = Paralect.Machine.Sockets.Socket;

namespace Paralect.Machine.Nodes
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
            Int64 previousJournalSeq = 0;
            Int64 currentJournalSeq = 0;
            

            using (Socket routerRepSocket = _context.CreateSocket(SocketType.PULL))
            using (Socket routerPubSocket = _context.CreateSocket(SocketType.PUB))
            using (Socket domainReqSocket = _context.CreateSocket(SocketType.PUSH))
            {
                // Bind and connect to sockets
                routerRepSocket.Bind(_routerRepAddress);
                routerPubSocket.Bind(_routerPubAddress);
                domainReqSocket.Connect(_domainReqAddress, token);

                // Process while canellation not requested
                while (!token.IsCancellationRequested)
                {
                    // Waits for binary envelopes (with timeout)
                    var packet = routerRepSocket.RecvPacket(200);
                    if (packet == null) continue;

                    // Ignore packets without messages
                    if (packet.Headers.ContentType != ContentType.Messages)
                        continue;

                    previousJournalSeq = currentJournalSeq;

                    // Journal all messages
                    currentJournalSeq = JournalPacket(packet);

                    // Publish all messages
                    PublishMessages(routerPubSocket, packet, currentJournalSeq);

                    // Router to process node
                    Route(domainReqSocket, "process", packet.CloneEnvelopes(), currentJournalSeq, previousJournalSeq);
                }
            }
        }

        private void Route(Socket socket, String routerName, IList<IPacketMessageEnvelope> envelopes, Int64 currentJournalSequence, Int64 previousJournalSequence)
        {
            var router = _context.RouterFactory.GetRouter(routerName);
            var outbox = router.Route(envelopes);

            if (outbox.Count == 0)
                return;

            var headers = new PacketHeaders()
            {
                CurrentJournalSequence = currentJournalSequence,
                PreviousJournalSequence = previousJournalSequence
            };

            var packet = _context.CreatePacket(outbox, headers);
            socket.SendPacket(packet);
        }

        private Int64 JournalPacket(IPacket packet)
        {
            // Journal all messages
            var seq = _storage.Save(packet.CloneEnvelopes());
            return seq;
        }

        private void PublishMessages(Socket routerPubSocket, IPacket packet, Int64 seq)
        {
            var clonedEnvelopes = packet.CloneEnvelopes();

            for (int i = 0; i < clonedEnvelopes.Count; i++)
            {
                var envelope = clonedEnvelopes[i];
                
                var metadata = envelope.Metadata;
                // Set sequence for each message
                metadata.JournalSequence = seq - clonedEnvelopes.Count + i + 1;

                var newPacket = _context.CreatePacket(b => b
                    .AddMessage(envelope.MessageBinary, metadata)
                );

                routerPubSocket.SendPacket(newPacket);
            }
        }

        public void Dispose()
        {
            
        }
    }
}