using System;
using System.Collections.Generic;
using System.Linq;

namespace Paralect.Machine.Messages
{
    /// <summary>
    /// Packet represents "physical", transport-level message.
    /// 
    /// Packet knows how to serialize/deserialize yourself. That makes possible, for example, to deserialize only packet's headers,
    /// without envelopes. 
    /// 
    /// There are two ways to create Packet:
    ///   1) By calling first constructor and passing multipart binary data in the form of IList(byte[])
    ///   2) By calling second constructor and passing IPacketHeader and IList(IMessageEnvelope)
    /// </summary>
    public class Packet : IPacket
    {
        /// <summary>
        /// Packet serializer that responsible for headers, envelopes, message and metadata 
        /// serialization/deserialization
        /// </summary>
        private readonly PacketSerializer _serializer;

        /// <summary>
        /// Packet headers contains transport-level metadata for each packet.
        /// </summary>
        private IPacketHeaders _headers;

        /// <summary>
        /// Message envelopes
        /// </summary>
        private IList<IMessageEnvelope> _envelopes;

        /// <summary>
        /// Packet headers in binary form
        /// </summary>
        private byte[] _headersBinary;

        /// <summary>
        /// Message envelope in binary form
        /// </summary>
        private IList<IMessageEnvelopeBinary> _envelopesBinary;

        /// <summary>
        /// Returns packet headers (deserialized, if needed).
        /// If headers are available only in binary form - headers will be deserialized and cached automatically. 
        /// Subsequent calls will use value from cache.
        /// </summary>
        public IPacketHeaders GetHeaders()
        {
            if (_headers == null)
            {
                _headers = GetHeadersCopy();
                _headersBinary = null;
            }

            return _headers;
        }

        /// <summary>
        /// Returns packet headers in binary form.
        /// If headers are not available in binary form - headers will be serialized and cached automatically.
        /// Subsequent calls will use value from cache.
        /// </summary>
        public byte[] GetHeadersBinary()
        {
            if (_headersBinary == null)
                _headersBinary = _serializer.SerializeHeaders(_headers);

            return _headersBinary;
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

        public Packet(PacketSerializer serializer, IPacketHeaders headers, IList<IMessageEnvelope> envelopes)
        {
            _serializer = serializer;
            _headers = headers;
            _envelopes = envelopes;
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