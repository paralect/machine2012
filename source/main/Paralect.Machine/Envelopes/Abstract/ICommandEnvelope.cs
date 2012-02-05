using Paralect.Machine.Messages;
using Paralect.Machine.Metadata;

namespace Paralect.Machine.Envelopes
{
    public interface ICommandEnvelope : IMessageEnvelope
    {
        new ICommandMetadata Metadata { get; set; }
        ICommand Command { get; set; }
    }
}