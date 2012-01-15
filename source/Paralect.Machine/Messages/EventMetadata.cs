using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public class EventMetadata : IEventMetadata
    {
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

    public class EventMetadata<TIdentity> : IEventMetadata<TIdentity>
        where TIdentity : IIdentity
    {
        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        IIdentity IEventMetadata.SenderId
        {
            get { return SenderId; }
            set { SenderId = (TIdentity)value; }
        }

        public TIdentity SenderId { get; set; }

        /// <summary>
        /// Version of Aggregate Root, Service or Process at the moment event was emitted.
        /// Emitting party should increment version and next event should have version incremented by one.
        /// Can be used to support commutativity of event handling operations.
        /// </summary>
        public int SenderVersion { get; set; }        
    }
}