namespace Paralect.Machine.Messages
{
    public class EventEnvelope : MessageEnvelope, IEventEnvelope
    {
        public EventEnvelope(IEventMetadata metadata, IEvent message) : base(metadata, message)
        {
        }

        IEventMetadata IEventEnvelope.GetMetadata()
        {
            return (IEventMetadata) GetMetadata();
        }

        public IEvent GetEvent()
        {
            return (IEvent) GetMessage();
        }
    }
}