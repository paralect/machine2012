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

        /// <summary>
        /// Returns list of cloned envelopes. Returned Envelopes are in binary state.
        /// </summary>
        IList<IMessageEnvelope> GetEnvelopesCloned();

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