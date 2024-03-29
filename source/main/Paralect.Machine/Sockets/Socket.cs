﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Paralect.Machine.Messages;
using ZMQ;

namespace Paralect.Machine.Sockets
{
    public class Socket : IDisposable
    {
        private MachineContext _context;
        private ZMQ.Socket _zmqSocket;

        public Socket(MachineContext context, ZMQ.Socket zmqSocket)
        {
            _context = context;
            _zmqSocket = zmqSocket;
        }

        public Queue<byte[]> RecvAll(Int32 timeout)
        {
            byte[] bytes = _zmqSocket.Recv(timeout);

            if (bytes == null)
                return null;

            var queue = new Queue<byte[]>();
            queue.Enqueue(bytes);

            while (_zmqSocket.RcvMore)
            {
                queue.Enqueue(_zmqSocket.Recv());
            }

            return queue;
        }

        public Queue<string> RecvAll(Encoding encoding, Int32 timeout)
        {
            string bytes = _zmqSocket.Recv(encoding, timeout);

            if (bytes == null)
                return null;

            var queue = new Queue<String>();
            queue.Enqueue(bytes);

            while (_zmqSocket.RcvMore)
            {
                queue.Enqueue(_zmqSocket.Recv(encoding));
            }

            return queue;
        }

        /// <summary>
        /// Fancy way to connect to socket insuring that connection accepted.
        /// Seems to make sence only for in-proc transport.
        /// </summary>
        public void Connect(String address, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    _zmqSocket.Connect(address);
                    return;
                }
                catch (ZMQ.Exception ex)
                {
                    // Connection refused
                    if (ex.Errno == 107)
                    {
                        SpinWait.SpinUntil(() => token.IsCancellationRequested, 200);
                        Thread.Sleep(20);
                        continue;
                    }

                    throw;
                }
            }
        }

        public IPacket RecvPacket(Int32 timeout)
        {
            var queue = RecvAll(timeout);
            if (queue == null) return null;

            return new Packet(_context.PacketSerializer, queue.ToList());
        }

        public SendStatus SendPacket(IPacket envelope)
        {
            var parts = new Queue<byte[]>(envelope.Serialize());

            // send Packet as multipart message
            while (parts.Count != 1)
                _zmqSocket.SendMore(parts.Dequeue());

            return _zmqSocket.Send(parts.Dequeue());
        }

        public void Bind(String address)
        {
            _zmqSocket.Bind(address);
        }

        public void Dispose()
        {
            if (_zmqSocket != null)
                _zmqSocket.Dispose();
        }
    }
}