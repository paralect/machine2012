using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public abstract class Event : IEvent
    {
        private EventMetadata _metadata = new EventMetadata();

        IEventMetadata IEvent.Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata) value; }
        }
    }

    public abstract class Event<TIdentity, TEventMetadata> : IEvent<TIdentity, TEventMetadata>
        where TIdentity : IIdentity
        where TEventMetadata : IEventMetadata<TIdentity>, new()
    {
        private TEventMetadata _metadata = new TEventMetadata();

        IEventMetadata IEvent.Metadata
        {
            get { return _metadata; }
            set { _metadata = (TEventMetadata)value; }
        }

        public TEventMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        IEventMetadata<TIdentity> IEvent<TIdentity>.Metadata
        {
            get { return _metadata; }
            set { _metadata = (TEventMetadata)value; }
        }
    }

    public abstract class Event<TIdentity> : Event<TIdentity, EventMetadata<TIdentity>>
        where TIdentity : IIdentity
    {

    }


}