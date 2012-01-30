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

namespace Paralect.Machine.Routers
{
    public class RouterNode : INode
    {
        private readonly Context _context;
        private readonly String _routerRepAddress;
        private readonly String _routerPubAddress;
        private readonly String _domainReqAddress;
        private readonly IJournalStorage _storage;

        public RouterNode(Context context, String routerRepAddress, String routerPubAddress, String domainReqAddress, IJournalStorage storage)
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
            using (Socket routerRepSocket = _context.Socket(SocketType.PULL))
            using (Socket routerPubSocket = _context.Socket(SocketType.PUB))
            using (Socket domainReqSocket = _context.Socket(SocketType.REQ))
            {
                // Bind and connect to sockets
                routerRepSocket.Bind(_routerRepAddress);
                routerPubSocket.Bind(_routerPubAddress);
                //domainReqSocket.EstablishConnect(_domainReqAddress, token);

                // Process while canellation not requested
                while (!token.IsCancellationRequested)
                {
                    // Waits for binary envelopes (with timeout)
                    var binaryEnvelope = routerRepSocket.RecvBinaryEnvelope(200);
                    if (binaryEnvelope == null) continue;

                    // Journal all messages
                    var seq = _storage.Save(binaryEnvelope.MessageEnvelopes);

                    for (int i = 0; i < binaryEnvelope.MessageEnvelopes.Count; i++)
                    {
                        var currentIndex = i;
                        var binaryMessageEnvelope = binaryEnvelope.MessageEnvelopes[i];

                        var outboxEnvelope = new BinaryEnvelope()
                            .AddBinaryMessageEnvelope(binaryMessageEnvelope, header =>
                            {
                                header.Set("Journal-Stream-Sequence", seq - binaryEnvelope.MessageEnvelopes.Count + currentIndex + 1);
                            });
                                

                        routerPubSocket.SendBinaryEnvelope(outboxEnvelope);
                    }
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}