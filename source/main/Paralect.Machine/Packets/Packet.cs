using System.Collections.Generic;
using Paralect.Machine.Envelopes;

namespace Paralect.Machine.Packets
{
    public class Packet : IPacket
    {
        private readonly PacketPartsSerializer _serializer;
        private IPacketHeaders _headers;
        private IEnumerable<IMessageEnvelope> _envelopes = new List<IMessageEnvelope>();

        private byte[] _headersBinary;
        private IEnumerable<byte[]> _envelopesBinary;

        public IPacketHeaders Headers
        {
            get
            {
                if (_headers == null)
                {
                    _headers = GetHeadersCopy();
                    _headersBinary = null;
                }

                return _headers;
            }
        }

        public IEnumerable<IMessageEnvelope> Envelopes
        {
            get
            {
                if (_envelopesBinary == null)
                {
                    _envelopes = GetEnvelopesCopy();
                    _envelopesBinary = null;
                }

                return _envelopes;
            }
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
            if (_headersBinary == null)
                _headersBinary = _serializer.SerializeHeaders(_headers);

            if (_envelopesBinary == null)
                _envelopesBinary = _serializer.Serialize(_envelopes);

            yield return _headersBinary;

            foreach (var part in _envelopesBinary)
            {
                yield return part;
            }
        }

        public Packet(PacketPartsSerializer serializer, byte[] headersBinary, IEnumerable<byte[]> envelopesBinary)
        {
            _serializer = serializer;
            _headersBinary = headersBinary;
            _envelopesBinary = envelopesBinary;
        }

        public Packet(PacketPartsSerializer serializer, IPacketHeaders headers, IEnumerable<byte[]> envelopesBinary)
        {
            _serializer = serializer;
            _headers = headers;
            _envelopesBinary = envelopesBinary;
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

        public static Packet FromQueue(Queue<byte[]> queue, PacketPartsSerializer serializer)
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