using System.Collections.Generic;

namespace Paralect.Machine.Packets
{
    public interface IPacket
    {
        /// <summary>
        /// Packet headers
        /// </summary>
        IPacketHeaders Headers { get; }

        /// <summary>
        /// Packets as a multipart data
        /// </summary>
        IEnumerable<byte[]> Parts { get; }
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