namespace Paralect.Machine.Messages
{
    public class CommandEnvelope : MessageEnvelope, ICommandEnvelope
    {
        public CommandEnvelope(ICommandMetadata metadata, ICommand message) : base(metadata, message)
        {
        }

        ICommandMetadata ICommandEnvelope.GetMetadata()
        {
            return (ICommandMetadata) GetMetadata();
        }

        public ICommand GetCommand()
        {
            return (ICommand) GetMessage();
        }
    }
}