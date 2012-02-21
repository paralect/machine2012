namespace Paralect.Machine.Messages
{
    public interface ICommandEnvelope : IMessageEnvelope
    {
        new ICommandMetadata Metadata { get; }
        ICommand Command { get; }
    }
}