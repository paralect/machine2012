using Paralect.Machine.Identities;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    public abstract class Event : IEvent
    {
        private EventMetadata _metadata = new EventMetadata();

        [ProtoMember(1)]
        public EventMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        #region Explicit implementation

        IEventMetadata IEvent.Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata) value; }
        }

        IMessageMetadata IMessage.Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata) value; }
        }

        #endregion
    }

    [ProtoContract]
    public abstract class Event<TSenderId> : IEvent<TSenderId>
        where TSenderId : IIdentity
    {
        private EventMetadata<TSenderId> _metadata = new EventMetadata<TSenderId>();

        [ProtoMember(1)]
        public EventMetadata<TSenderId> Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        #region Explicit implementation

        IMessageMetadata IMessage.Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata<TSenderId>) value; }
        }

        IEventMetadata IEvent.Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata<TSenderId>) value; }
        }

        IEventMetadata<TSenderId> IEvent<TSenderId>.Metadata
        {
            get { return _metadata; }
            set { _metadata = (EventMetadata<TSenderId>) value; }
        }

        #endregion
    }

    /// <summary>
    /// With default EventMetadata
    /// </summary>
    /// <typeparam name="TIdentity"></typeparam>
/*    public abstract class Event<TIdentity> : Event<TIdentity, EventMetadata<TIdentity>>
        where TIdentity : IIdentity
    {

    }*/
}