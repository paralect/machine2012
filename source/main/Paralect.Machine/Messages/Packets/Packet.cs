using System.Collections.Generic;
using System.Linq;

namespace Paralect.Machine.Messages
{
    public class Packet : IPacket
    {
        private readonly PacketSerializer _serializer;
        private IPacketHeaders _headers;
        private IEnumerable<IMessageEnvelope> _envelopes;

        private byte[] _headersBinary;
        private IEnumerable<byte[]> _envelopesBinary;

        public IPacketHeaders GetHeaders()
        {
            if (_headers == null)
            {
                _headers = GetHeadersCopy();
                _headersBinary = null;
            }

            return _headers;
        }

        public IEnumerable<IMessageEnvelope> GetEnvelopes()
        {
            if (_envelopes == null)
            {
                _envelopes = GetEnvelopesCopy();
                _envelopesBinary = null;
            }

            return _envelopes;
        }

        public byte[] GetHeadersBinary()
        {
            if (_headersBinary == null)
                _headersBinary = _serializer.SerializeHeaders(_headers);

            return _headersBinary;
        }

        public IEnumerable<byte[]> GetEnvelopesBinary()
        {
            if (_envelopesBinary == null)
                _envelopesBinary = _serializer.Serialize(_envelopes);

            return _envelopesBinary;
        }

        public IPacketHeaders GetHeadersCopy()
        {
            return _serializer.DeserializeHeaders(_headersBinary);
        }

        public IEnumerable<IMessageEnvelope> GetEnvelopesCopy()
        {
            return _serializer.Deserialize(_envelopesBinary);
        }

        public IEnumerable<byte[]> Serialize()
        {
            yield return GetHeadersBinary();

            foreach (var part in GetEnvelopesBinary())
            {
                yield return part;
            }
        }

        public Packet(PacketSerializer serializer, byte[] headersBinary, IEnumerable<byte[]> envelopesBinary)
        {
            _serializer = serializer;
            _headersBinary = headersBinary;
            _envelopesBinary = envelopesBinary;
        }

        public Packet(PacketSerializer serializer, IEnumerable<byte[]> parts)
        {
            var list = parts.ToList();
            _serializer = serializer;
            _headersBinary = list[0];
            _envelopesBinary = list.GetRange(1, list.Count - 1);
        }

        public Packet(PacketSerializer serializer, IPacketHeaders headers, IEnumerable<IMessageEnvelope> envelopes)
        {
            _serializer = serializer;
            _headers = headers;
            _envelopes = envelopes;
        }

        public Queue<byte[]> ToQueue()
        {
            var queue = new Queue<byte[]>();
            var data = Serialize();

            foreach (var part in data)
            {
                queue.Enqueue(part);
            }

            return queue;
        }

        public static Packet FromQueue(Queue<byte[]> queue, PacketSerializer serializer)
        {
            var headers = queue.Dequeue();
            List<byte[]> parts = new List<byte[]>();

            while (queue.Count != 0)
            {
                parts.Add(queue.Dequeue());
            }

            return new Packet(serializer, headers, parts);
        }
    }
}