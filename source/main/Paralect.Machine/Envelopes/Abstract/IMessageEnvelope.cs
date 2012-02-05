using Paralect.Machine.Messages;
using Paralect.Machine.Metadata;

namespace Paralect.Machine.Envelopes
{
    public interface IMessageEnvelope
    {
        IMessageMetadata Metadata { get; }
        IMessage Message { get; } 
    }
}