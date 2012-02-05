using System.Collections.Generic;

namespace Paralect.Machine.Packets
{
    public class Packet : IPacket
    {
        private readonly PacketPartsSerializer _serializer;
        private IPacketHeaders _headers;

        private byte[] _headersBinary;
        private readonly IEnumerable<byte[]> _partsBinary;

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

        public IPacketHeaders GetHeadersCopy()
        {
            return _serializer.DeserializeHeaders(_headersBinary);
        }

        public IEnumerable<byte[]> Serialize()
        {
            if (_headersBinary == null)
                _headersBinary = _serializer.SerializeHeaders(_headers);

            yield return _headersBinary;

            foreach (var part in _partsBinary)
            {
                yield return part;
            }
        }

        public IEnumerable<byte[]> Parts
        {
            get { return _partsBinary;  }
        }

        public Packet(PacketPartsSerializer serializer, byte[] headersBinary, IEnumerable<byte[]> partsBinary)
        {
            _serializer = serializer;
            _headersBinary = headersBinary;
            _partsBinary = partsBinary;
        }

        public Packet(PacketPartsSerializer serializer, IPacketHeaders headers, IEnumerable<byte[]> partsBinary)
        {
            _serializer = serializer;
            _headers = headers;
            _partsBinary = partsBinary;
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