using System;
using Paralect.Machine.Identities;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    public class EventMetadata : IEventMetadata
    {
        /// <summary>
        /// Unique Message identity
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// ID of message that was a stimulus to produce this message.
        /// If there is no stimulus for this message, then TriggerMessageId should return Guid.Empty.
        /// </summary>
        public Guid TriggerMessageId { get; set; }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Lamport_timestamps
        /// </summary>
        public long LamportTimestamp { get; set; }

        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        public IIdentity SenderId { get; set; }

        /// <summary>
        /// Version of Aggregate Root, Service or Process at the moment event was emitted.
        /// Emitting party should increment version and next event should have version incremented by one.
        /// Can be used to support commutativity of event handling operations.
        /// </summary>
        public int SenderVersion { get; set; }
    }


    [ProtoContract]
    public class EventMetadata<TSenderId> : IEventMetadata<TSenderId>
        where TSenderId : IIdentity
    {
        /// <summary>
        /// Message ID
        /// </summary>
        [ProtoMember(1)]
        public Guid MessageId { get; set; }

        /// <summary>
        /// ID of message that was a stimulus to produce this message.
        /// If there is no stimulus for this message, then TriggerMessageId should return Guid.Empty.
        /// TriggerMessageId allows partially to restore causality of messages.
        /// </summary>
        [ProtoMember(2)]
        public Guid TriggerMessageId { get; set; }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Lamport_timestamps
        /// </summary>
        [ProtoMember(3)]
        public long LamportTimestamp { get; set; }

        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        [ProtoMember(4)]
        public TSenderId SenderId { get; set; }

        /// <summary>
        /// Version of Aggregate Root, Service or Process at the moment event was emitted.
        /// Emitting party should increment version and next event should have version incremented by one.
        /// Can be used to support commutativity of event handling operations.
        /// </summary>
        [ProtoMember(5)]
        public int SenderVersion { get; set; }


        #region Explicit implementation

        /// <summary>
        /// Unique Message identity
        /// </summary>
        Guid IMessageMetadata.MessageId
        {
            get { return MessageId; }
            set { MessageId = value; }
        }

        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        IIdentity IEventMetadata.SenderId
        {
            get { return SenderId; }
            set { SenderId = (TSenderId)value; }
        }

        #endregion  
    }
}