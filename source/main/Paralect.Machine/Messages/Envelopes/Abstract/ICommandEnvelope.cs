namespace Paralect.Machine.Messages
{
    public interface ICommandEnvelope : IMessageEnvelope
    {
        new ICommandMetadata GetMetadata();
        ICommand GetCommand();
    }
}