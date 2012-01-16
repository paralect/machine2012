using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface IEvent : IMessage
    {
        new IEventMetadata Metadata { get; set; }
    }

    public interface IEvent<TSenderId> : IEvent
    {
        new IEventMetadata<TSenderId> Metadata { get; set; } 
    }

    public interface IEvent<TMessageId, TSenderId, TEventMetadata> : IEvent<TSenderId>
        where TMessageId : IIdentity
        where TSenderId : IIdentity
        where TEventMetadata : IEventMetadata<TMessageId, TSenderId>
    {
        new TEventMetadata Metadata { get; set; }
    }
}