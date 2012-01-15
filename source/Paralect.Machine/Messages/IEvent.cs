using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface IEvent : IMessage
    {
        IEventMetadata Metadata { get; set; }
    }

    public interface IEvent<TIdentity> : IEvent
        where TIdentity : IIdentity
    {
        new IEventMetadata<TIdentity> Metadata { get; set; }
    }

    public interface IEvent<TIdentity, TEventMetadata> : IEvent<TIdentity>
        where TIdentity : IIdentity
        where TEventMetadata : IEventMetadata<TIdentity>
    {
        new TEventMetadata Metadata { get; set; }
    }
}