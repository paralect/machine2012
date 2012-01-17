using System;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface IMessageMetadata
    {
        /// <summary>
        /// Unique Message identity
        /// </summary>
        Guid MessageId { get; set; }

        /// <summary>
        /// ID of message that was a stimulus to produce this message.
        /// If there is no stimulus for this message, then TriggerMessageId should return Guid.Empty.
        /// TriggerMessageId allows partially to restore causality of messages.
        /// </summary>
        Guid TriggerMessageId { get; set; }

        /// <summary>
        /// Lamport Timestamp allows partial ordering of messages in distributed computer system.
        /// 
        /// Here is a simple rules that allows to achieve partial ordering:
        /// 
        /// 1. A process increments its counter before each event in that process;
        /// 2. When a process sends a message, it includes its counter value with the message;
        /// 3. On receiving a message, the receiver process sets its counter to be greater than 
        ///    the maximum of its own value and the received value before it considers the message received.
        /// 
        /// More info:
        /// http://en.wikipedia.org/wiki/Lamport_timestamps
        /// </summary>
        Int64 LamportTimestamp { get; set; }
    }
}