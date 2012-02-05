using System;
using System.Collections.Generic;
using System.Linq;

namespace Paralect.Machine.Messages
{
    public class Packet : IPacket
    {
        private readonly PacketSerializer _serializer;
        private IPacketHeaders _headers;
        private IList<IMessageEnvelope> _envelopes;

        private byte[] _headersBinary;
        private IList<IMessageEnvelopeBinary> _envelopesBinary;

        public IPacketHeaders GetHeaders()
        {
            if (_headers == null)
            {
                _headers = GetHeadersCopy();
                _headersBinary = null;
            }

            return _headers;
        }

        public IList<IMessageEnvelope> GetEnvelopes()
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

        public IList<IMessageEnvelopeBinary> GetEnvelopesBinary()
        {
            if (_envelopesBinary == null)
                _envelopesBinary = _serializer.Serialize(_envelopes);

            return _envelopesBinary;
        }

        public IPacketHeaders GetHeadersCopy()
        {
            return _serializer.DeserializeHeaders(_headersBinary);
        }

        public IList<IMessageEnvelope> GetEnvelopesCopy()
        {
            return _serializer.Deserialize(_envelopesBinary);
        }

        public IList<byte[]> Serialize()
        {
            var list = new List<byte[]>();

            list.Add(GetHeadersBinary());

            foreach (var part in GetEnvelopesBinary())
            {
                list.Add(part.GetMetadataBinary());
                list.Add(part.GetMessageBinary());
            }

            return list.AsReadOnly();
        }

        public Packet(PacketSerializer serializer, IList<byte[]> parts)
        {
            _serializer = serializer;
            _headersBinary = parts[0];
            _envelopesBinary = EnvelopePartsToEnvelopeBinary(parts.Skip(1).ToList());
        }

        private IList<IMessageEnvelopeBinary> EnvelopePartsToEnvelopeBinary(IList<byte[]> envelopeParts)
        {
            if (envelopeParts.Count % 2 != 0)
                throw new Exception("Incorrect number of envelope parts");

            var list = new List<IMessageEnvelopeBinary>(envelopeParts.Count / 2);

            for (int i = 0 ; i < envelopeParts.Count / 2; i++)
            {
                var binary = new MessageEnvelopeBinary(
                    envelopeParts[i * 2 + 1],
                    envelopeParts[i * 2]);

                list.Add(binary);
            }

            return list.AsReadOnly();
        }

        public Packet(PacketSerializer serializer, IPacketHeaders headers, IList<IMessageEnvelope> envelopes)
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
            return new Packet(serializer, queue.ToList());
        }
    }
}