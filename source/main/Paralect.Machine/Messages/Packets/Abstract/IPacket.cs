using System;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    public interface IPacket
    {
        /// <summary>
        /// Returns packet headers (deserialized, if needed).
        /// </summary>
        IPacketHeaders GetHeaders();

        /// <summary>
        /// Returns packet headers in binary form.
        /// </summary>
        Byte[] GetHeadersBinary();

        /// <summary>
        /// Message envelopes
        /// </summary>
        IList<IMessageEnvelope> GetEnvelopes();

        IList<IMessageEnvelope> GetEnvelopesCloned();

        IList<Byte[]> Serialize();
        
    }

    public enum ContentType
    {
        Messages = 0,
        States = 1
    }
}