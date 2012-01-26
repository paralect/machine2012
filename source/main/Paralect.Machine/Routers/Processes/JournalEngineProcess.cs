using System;
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
        private readonly String _address;

        public RouterEngineProcess(MessageFactory messageFactory, ProtobufSerializer serializer, Context context, String address)
        {
            _messageFactory = messageFactory;
            _serializer = serializer;
            _context = context;
            _address = address;
        }

        public void Initialize()
        {
            
        }

        public Task Start(CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var envelopeSerializer = new EnvelopeSerializer(_serializer, _messageFactory.TagToTypeResolver);

                    using (var socket = _context.Socket(SocketType.REP))
                    {
                        socket.Connect(_address);

                        Console.WriteLine();
                        Console.WriteLine("Router process has connected");

                        while (!token.IsCancellationRequested)
                        {
                            var bytes = socket.Recv(200);

                            if (bytes == null)
                                continue;

/*                            // Answer that messages journaled successfully
                            var message = new MessagesJournaledSuccessfully {Sequence = seq};
                            var answerBytes = new EnvelopeBuilder(_messageFactory.TypeToTagResolver)
                                .AddMessage(message)
                                .BuildAndSerialize(envelopeSerializer);*/

                          //  socket.Send(answerBytes);
                        }

                        Console.WriteLine("Done with server");
                        
                    }
                }
                catch (ObjectDisposedException)
                {
                    // suppress
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
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