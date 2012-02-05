using Paralect.Machine.Messages;
using Paralect.Machine.Metadata;

namespace Paralect.Machine.Envelopes
{
    public interface IEventEnvelope : IMessageEnvelope
    {
        new IEventMetadata Metadata { get; set; }
        IEvent Event { get; set; }
    }
}