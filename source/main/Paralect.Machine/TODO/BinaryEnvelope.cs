using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Paralect.Machine.Messages
{
    public class BinaryEnvelope
    {
        public byte[] Header { get; set; }
        public IList<BinaryMessageEnvelope> MessageEnvelopes { get; set; }

        public BinaryEnvelope()
        {
            MessageEnvelopes = new List<BinaryMessageEnvelope>();
            Header = new byte[0];
        }

        public BinaryEnvelope AddBinaryMessageEnvelope(BinaryMessageEnvelope binaryMessageEnvelope)
        {
            MessageEnvelopes.Add(binaryMessageEnvelope);
            return this;
        }

        public BinaryEnvelope AddBinaryMessageEnvelope(BinaryMessageEnvelope binaryMessageEnvelope, Action<Header> headerMutation)
        {
            binaryMessageEnvelope.MutateHeader(headerMutation);
            return AddBinaryMessageEnvelope(binaryMessageEnvelope);
        }

        public Queue<byte[]> ToQueue()
        {
            var queue = new Queue<byte[]>();

            queue.Enqueue(Header);
            foreach (var binaryMessageEnvelope in MessageEnvelopes)
            {
                queue.Enqueue(binaryMessageEnvelope.Header);
                queue.Enqueue(binaryMessageEnvelope.Message);
            }

            return queue;
        }

        public static BinaryEnvelope FromQueue(Queue<byte[]> queue)
        {
            var binary = new BinaryEnvelope();
            binary.Header = queue.Dequeue();

            while (queue.Count != 0)
            {
                var binaryMessageEnvelope = new BinaryMessageEnvelope();
                binaryMessageEnvelope.Header = queue.Dequeue();
                binaryMessageEnvelope.Message = queue.Dequeue();
                binary.AddBinaryMessageEnvelope(binaryMessageEnvelope);
            }

            return binary;
        }

        public void MutateMessageHeaders(Action<Header, Int32> messageHeaderMutation)
        {
            for (int i = 0; i < MessageEnvelopes.Count; i++)
            {
                var binaryMessageEnvelope = MessageEnvelopes[i];
                binaryMessageEnvelope.MutateHeader(
                    header => messageHeaderMutation(header, i)
                );
            }
        }
    }
}