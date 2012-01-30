using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface IEvent : IMessage
    {
    }

    public interface IEvent<TSenderId> : IEvent
    {
        //TSenderId SenderId { get; set; }
    }
}