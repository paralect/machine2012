namespace Paralect.Machine.Messages
{
    public class CommandEnvelope : MessageEnvelope, ICommandEnvelope
    {
        public CommandEnvelope(PacketSerializer serializer, ICommandMetadata metadata, ICommand message) : base(serializer, metadata, message)
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