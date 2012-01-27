using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Paralect.Machine.Engine;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;
using Paralect.Machine.Serialization;
using ZMQ;

namespace Paralect.Machine.Routers
{
    public class RouterEngineProcess : IEngineProcess
    {
        private readonly MessageFactory _messageFactory;
        private readonly ProtobufSerializer _serializer;
        private readonly Context _context;
        private readonly String _routerRepAddress;
        private readonly String _routerPubAddress;
        private readonly String _domainReqAddress;
        private readonly IJournalStorage _storage;

        public RouterEngineProcess(MessageFactory messageFactory, ProtobufSerializer serializer, Context context,
            String routerRepAddress, String routerPubAddress, String domainReqAddress, 
            IJournalStorage storage)
        {
            _messageFactory = messageFactory;
            _serializer = serializer;
            _context = context;
            _routerRepAddress = routerRepAddress;
            _routerPubAddress = routerPubAddress;
            _domainReqAddress = domainReqAddress;
            _storage = storage;
        }

        public void Initialize()
        {
            
        }

        public Task Start(CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                Socket routerRepSocket = _context.Socket(SocketType.REP);
                Socket routerPubSocket = _context.Socket(SocketType.PUB);
                Socket domainReqSocket = _context.Socket(SocketType.REQ);

                try
                {
                    var envelopeSerializer = new EnvelopeSerializer(_serializer, _messageFactory.TagToTypeResolver);

                    routerRepSocket.Bind(_routerRepAddress);
                    routerPubSocket.Bind(_routerPubAddress);

                    #region Fancy way to connect to domain process
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            domainReqSocket.Connect(_domainReqAddress);
                            break;
                        }
                        catch (ZMQ.Exception ex)
                        {
                            // Connection refused
                            if (ex.Errno == 107)
                            {
                                SpinWait.SpinUntil(() => token.IsCancellationRequested, 200);
                                continue;
                            }

                            throw;
                        }
                    }
                    #endregion


                    while (!token.IsCancellationRequested)
                    {
                        var bytes = routerRepSocket.Recv(200);

                        if (bytes == null)
                            continue;

                        var queue = new Queue<byte[]>();
                        queue.Enqueue(bytes);

                        while (routerRepSocket.RcvMore)
                        {
                            queue.Enqueue(routerRepSocket.Recv());
                        }

                        // Journal messages
                        Envelope envelope = envelopeSerializer.Deserialize(BinaryEnvelope.FromQueue(queue));
                        //var seq = _storage.Save(envelope.Items);

                        var index = 0;
                        foreach (var messageEnvelope in envelope.Items)
                        {
                            var messageSequence = /*seq */ 242 - envelope.ItemsCount + index + 1;
                            messageEnvelope.Header.AddMetadata("Sequence", messageSequence.ToString());
                            index++;
                        }


                        foreach (var messageEnvelope in envelope.Items)
                        {
                            var messageBytes = new EnvelopeBuilder(_messageFactory.TypeToTagResolver)
                                .AddMessageEnvelope(messageEnvelope)
                                .BuildAndSerialize(envelopeSerializer);

                            var parts = messageBytes.ToQueue();


                            // send BinaryEnvelope as multipart message
                            while (parts.Count != 1)
                                routerPubSocket.SendMore(parts.Dequeue());

                            routerPubSocket.Send(parts.Dequeue());
                        }
                    }

                    Console.WriteLine("Done with server");
                }
                catch (ObjectDisposedException)
                {
                    // suppress
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (routerRepSocket != null)
                        routerRepSocket.Dispose();

                    if (routerPubSocket != null)
                        routerPubSocket.Dispose();

                    if (domainReqSocket != null)
                        domainReqSocket.Dispose();
                }
            }, token);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            
        }
    }
}