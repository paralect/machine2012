namespace Paralect.Machine.Messages
{
    public class EventEnvelope : MessageEnvelope, IEventEnvelope
    {
        public EventEnvelope(PacketSerializer serializer, IEventMetadata metadata, IEvent message) : base(serializer, metadata, message)
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