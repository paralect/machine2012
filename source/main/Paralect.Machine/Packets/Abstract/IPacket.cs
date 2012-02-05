using System.Collections.Generic;
using Paralect.Machine.Envelopes;

namespace Paralect.Machine.Packets
{
    public interface IPacket
    {
        /// <summary>
        /// Packet headers
        /// </summary>
        IPacketHeaders Headers { get; }

        IEnumerable<IMessageEnvelope> Envelopes { get; }
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