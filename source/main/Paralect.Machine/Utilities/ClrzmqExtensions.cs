using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Paralect.Machine.Messages;
using ZMQ;

namespace Paralect.Machine.Utilities
{
    public static class ClrzmqExtensions
    {
        public static Queue<byte[]> RecvAll(this Socket socket, Int32 timeout)
        {
            byte[] bytes = socket.Recv(timeout);

            if (bytes == null)
                return null;

            var queue = new Queue<byte[]>();
            queue.Enqueue(bytes);

            while (socket.RcvMore)
            {
                queue.Enqueue(socket.Recv());
            }

            return queue;
        }        
        
        public static Queue<string> RecvAll(this Socket socket, Encoding encoding, Int32 timeout)
        {
            string bytes = socket.Recv(encoding, timeout);

            if (bytes == null)
                return null;

            var queue = new Queue<String>();
            queue.Enqueue(bytes);

            while (socket.RcvMore)
            {
                queue.Enqueue(socket.Recv(encoding));
            }

            return queue;
        }

        /// <summary>
        /// Fancy way to connect to socket insuring that connection accepted.
        /// Seems to make sence only for in-proc transport.
        /// </summary>
        public static void EstablishConnect(this Socket socket, String address, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    socket.Connect(address);
                    return;
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
        }

        public static BinaryEnvelope RecvBinaryEnvelope(this Socket socket, Int32 timeout)
        {
            var queue = socket.RecvAll(timeout);
            if (queue == null) return null;

            return BinaryEnvelope.FromQueue(queue);
        }

        public static SendStatus SendBinaryEnvelope(this Socket socket, BinaryEnvelope envelope)
        {
            var parts = envelope.ToQueue();


            // send BinaryEnvelope as multipart message
            while (parts.Count != 1)
                socket.SendMore(parts.Dequeue());

            return socket.Send(parts.Dequeue());
        }
    }
}