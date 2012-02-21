namespace Paralect.Machine.Messages
{
    public interface IEventEnvelope : IMessageEnvelope
    {
        new IEventMetadata Metadata { get; }
        IEvent Event { get; }
    }
}