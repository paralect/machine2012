using System;
using System.Diagnostics;
using System.Threading;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;
using Paralect.Machine.Nodes;
using ZMQ;
using Socket = Paralect.Machine.Sockets.Socket;

namespace Paralect.Machine.Routers
{
    public class ProcessesNode : INode
    {
        private readonly MachineContext _context;
        private readonly String _processPullAddress;
        private readonly String _routerPushAddress;

        public ProcessesNode(MachineContext context, String processPullAddress, String routerPushAddress)
        {
            _context = context;
            _processPullAddress = processPullAddress;
            _routerPushAddress = routerPushAddress;
        }

        public void Init()
        {
            
        }

        public void Run(CancellationToken token)
        {
            using (Socket processPullSocket = _context.CreateSocket(SocketType.PULL))
            using (Socket routerPushSocket = _context.CreateSocket(SocketType.PUSH))
            {
                // Bind and connect to sockets
                processPullSocket.Bind(_processPullAddress);
                routerPushSocket.Connect(_routerPushAddress, token);

                // Process while canellation not requested
                while (!token.IsCancellationRequested)
                {
                    // Waits for binary envelopes (with timeout)
                    var packet = processPullSocket.RecvPacket(200);
                    if (packet == null) continue;

                    // Ignore packets without messages
                    if (packet.GetHeaders().ContentType != ContentType.Messages)
                        continue;

                    foreach (var envelope in packet.GetEnvelopes())
                    {
                        //Console.WriteLine("Envelope accepted : {0}. Now = {1}", envelope.GetMessage().ToString(), DateTime.Now.Millisecond);
                    }

                    // Publish all messages
                    //PublishMessages(routerPushSocket, packet, 1);
                }
            }
        }

        private void PublishMessages(Socket routerPubSocket, IPacket packet, Int64 seq)
        {
            var clonedEnvelopes = packet.GetEnvelopesCopy();

            for (int i = 0; i < clonedEnvelopes.Count; i++)
            {
                var envelope = clonedEnvelopes[i];
                
                var metadata = envelope.GetMetadata();
                // Set sequence for each message
                metadata.JournalSequence = seq - clonedEnvelopes.Count + i + 1;

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