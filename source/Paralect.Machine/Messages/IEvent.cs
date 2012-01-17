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

    public interface IEvent<TSenderId, TEventMetadata> : IEvent<TSenderId>
        where TSenderId : IIdentity
        where TEventMetadata : IEventMetadata<TSenderId>
    {
        new TEventMetadata Metadata { get; set; }
    }
}