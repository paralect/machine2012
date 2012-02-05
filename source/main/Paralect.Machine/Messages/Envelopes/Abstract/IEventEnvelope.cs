namespace Paralect.Machine.Messages
{
    public interface IEventEnvelope : IMessageEnvelope
    {
        new IEventMetadata GetMetadata();
        IEvent GetEvent();
    }
}