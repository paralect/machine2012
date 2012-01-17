using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public abstract class Event : IEvent
    {
        private EventMetadata _metadata = new EventMetadata();

        public IEventMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata) value; }
        }

        IMessageMetadata IMessage.Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata) value; }
        }
    }

    public abstract class Event<TSenderId, TEventMetadata> : IEvent<TSenderId, TEventMetadata>
        where TSenderId : IIdentity
        where TEventMetadata : IEventMetadata<TSenderId>, new()
    {
        private TEventMetadata _metadata = new TEventMetadata();

        public TEventMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        #region Explicit implementation

        IMessageMetadata IMessage.Metadata
        {
            get { return _metadata; }
            set { _metadata = (TEventMetadata) value; }
        }

        IEventMetadata IEvent.Metadata
        {
            get { return _metadata; }
            set { _metadata = (TEventMetadata) value; }
        }

        IEventMetadata<TSenderId> IEvent<TSenderId>.Metadata
        {
            get { return _metadata; }
            set { _metadata = (TEventMetadata) value; }
        }

        #endregion
    }

/*    public abstract class Event<TIdentity> : Event<GuidId, TIdentity, EventMetadata<GuidId, TIdentity>>
        where TIdentity : IIdentity
    {

    }

*/
}