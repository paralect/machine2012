﻿using System;
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
        /// Packet headers in binary form
        /// </summary>
        private byte[] _headersBinary;

        /// <summary>
        /// Message envelopes
        /// </summary>
        private readonly IList<IPacketMessageEnvelope> _envelopes;

        /// <summary>
        /// Returns packet headers (deserialized, if needed).
        /// If headers are available only in binary form - headers will be deserialized and cached automatically. 
        /// Subsequent calls will use value from cache.
        /// </summary>
        public IPacketHeaders Headers
        {
            get
            {
                if (_headers == null)
                {
                    _headers = _serializer.DeserializePacketHeaders(_headersBinary);
                    _headersBinary = null;
                }

                return _headers;
            }
        }

        /// <summary>
        /// Returns packet headers in binary form.
        /// If headers are not available in binary form - headers will be serialized and cached automatically.
        /// Subsequent calls will use value from cache.
        /// </summary>
        public byte[] HeadersBinary
        {
            get
            {
                if (_headersBinary == null)
                {
                    _headersBinary = _serializer.SerializePacketHeaders(_headers);
                    _headers = null;
                }

                return _headersBinary;
            }
        }

        /// <summary>
        /// Returns list of message envelopes. There is no message deserializations in this method.
        /// </summary>
        public IList<IPacketMessageEnvelope> Envelopes
        {
            get
            {
                return _envelopes;
            }
        }

        /// <summary>
        /// Returns list of cloned envelopes. Returned Envelopes are in binary state. Original packet has no relations 
        /// with the cloned envelopes.  Any changes to cloned envelopes does not impact any change 
        /// to original packet-owned envelopes. This method shows best possible performance when Envelopes were 
        /// already in binary state. If this is not a case - original envelopes will be serialized before cloning.
        /// </summary>
        public IList<IPacketMessageEnvelope> CloneEnvelopes()
        {
            var cloned = new List<IPacketMessageEnvelope>();

            foreach (var envelope in _envelopes)
                cloned.Add(envelope.CloneEnvelope());

            return cloned.AsReadOnly();
        }

        /// <summary>
        /// Builds multipart message in the form of list of byte array. 
        /// If there was no access to Packet Header, Message Metadata or Message - no serialization involved here,
        /// otherwise Packet Header, Message Metadata and Message will be serialized automatically.
        /// </summary>
        public IList<byte[]> Serialize()
        {
            var list = new List<byte[]>();

            list.Add(HeadersBinary);

            foreach (var part in Envelopes)
            {
                list.Add(part.MetadataBinary);
                list.Add(part.MessageBinary);
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Creates packet from the list binary data (multipart message)
        /// </summary>
        public Packet(PacketSerializer serializer, IList<byte[]> parts)
        {
            _serializer = serializer;
            _headersBinary = parts[0];
            _envelopes = EnvelopePartsToEnvelope(serializer, parts.Skip(1).ToList());
        }

        /// <summary>
        /// Creates packet with specified header and list of message envelopes
        /// </summary>
        public Packet(PacketSerializer serializer, IPacketHeaders headers, IList<IPacketMessageEnvelope> envelopes)
        {
            _serializer = serializer;
            _headers = headers;
            _envelopes = envelopes;
        }

        /// <summary>
        /// Helper method that builds list of message envelopes from list of multipart data in binary form.
        /// Returns list of Message Envelopes.
        /// </summary>
        private IList<IPacketMessageEnvelope> EnvelopePartsToEnvelope(PacketSerializer serializer, IList<byte[]> envelopeParts)
        {
            // Check that number of parts is even
            if (envelopeParts.Count % 2 != 0)
                throw new Exception("Incorrect number of envelope parts");

            // One message envelope consists of two binary parts - metadata part and message part
            var list = new List<IPacketMessageEnvelope>(envelopeParts.Count / 2);

            // Build message envelope
            for (int i = 0 ; i < envelopeParts.Count / 2; i++)
            {
                var envelope = new PacketMessageEnvelope(
                    serializer,
                    envelopeParts[i * 2 + 1],
                    envelopeParts[i * 2]);

                list.Add(envelope);
            }

            // Prevent modifications of this list.
            // If you want to modify packet's list of envelopes - create new packet.
            return list.AsReadOnly();
        }
    }
}