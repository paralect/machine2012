using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public class EventMetadata : IEventMetadata
    {
        /// <summary>
        /// Unique Message identity
        /// </summary>
        public IIdentity MessageId { get; set; }

        /// <summary>
        /// ID of message that was a stimulus to produce this message.
        /// If there is no stimulus for this message, then TriggerMessageId should return Guid.Empty.
        /// </summary>
        public IIdentity TriggerMessageId { get; set; }

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



    public class EventMetadata<TMessageId, TSenderId> : IEventMetadata<TMessageId, TSenderId>
        where TMessageId : IIdentity
        where TSenderId : IIdentity
    {
        /// <summary>
        /// Message ID
        /// </summary>
        public TMessageId MessageId { get; set; }

        /// <summary>
        /// ID of message that was a stimulus to produce this message.
        /// If there is no stimulus for this message, then TriggerMessageId should return Guid.Empty.
        /// TriggerMessageId allows partially to restore causality of messages.
        /// </summary>
        public IIdentity TriggerMessageId { get; set; }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Lamport_timestamps
        /// </summary>
        public long LamportTimestamp { get; set; }

        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        public TSenderId SenderId { get; set; }

        /// <summary>
        /// Version of Aggregate Root, Service or Process at the moment event was emitted.
        /// Emitting party should increment version and next event should have version incremented by one.
        /// Can be used to support commutativity of event handling operations.
        /// </summary>
        public int SenderVersion { get; set; }


        #region Explicit implementation

        /// <summary>
        /// Unique Message identity
        /// </summary>
        IIdentity IMessageMetadata.MessageId
        {
            get { return MessageId; }
            set { MessageId = (TMessageId)value; }
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