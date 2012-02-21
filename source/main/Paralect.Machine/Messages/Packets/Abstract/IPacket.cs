using System;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    public interface IPacket
    {
        /// <summary>
        /// Returns packet headers (deserialized, if needed).
        /// </summary>
        IPacketHeaders Headers { get; }

        /// <summary>
        /// Returns packet headers in binary form.
        /// </summary>
        byte[] HeadersBinary { get; }

        /// <summary>
        /// Message envelopes
        /// </summary>
        IList<IPacketMessageEnvelope> Envelopes { get; }

        /// <summary>
        /// Returns list of cloned envelopes. Returned Envelopes are in binary state.
        /// </summary>
        IList<IPacketMessageEnvelope> CloneEnvelopes();

        /// <summary>
        /// Builds multipart message in the form of list of byte array. 
        /// </summary>
        IList<Byte[]> Serialize();
    }

    public enum ContentType
    {
        Messages = 0,
        States = 1
    }
}