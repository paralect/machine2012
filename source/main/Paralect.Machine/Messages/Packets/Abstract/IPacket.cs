using System;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    public interface IPacket
    {
        /// <summary>
        /// Packet headers
        /// </summary>
        IPacketHeaders GetHeaders();

        /// <summary>
        /// Message envelopes
        /// </summary>
        IEnumerable<IMessageEnvelope> GetEnvelopes();

        Byte[] GetHeadersBinary();
        
        IEnumerable<Byte[]> GetEnvelopesBinary();

        IEnumerable<Byte[]> Serialize();
    }

    public interface IPacketHeaders
    {
        ContentType ContentType { get; set; }
    }

    public enum ContentType
    {
        Messages,
        States
    }
}