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
        IList<IMessageEnvelope> GetEnvelopes();

        Byte[] GetHeadersBinary();
        
        IList<IMessageEnvelopeBinary> GetEnvelopesBinary();

        IList<Byte[]> Serialize();
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