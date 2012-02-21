using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    public class SendResult : IResult
    {
        private readonly ICommand _command;

        public ICommand Command
        {
            get { return _command; }
        }

        public SendResult(ICommand command)
        {
            this._command = command;
        }

        public IEnumerable<IMessageEnvelope> BuildMessages(IMessage inputCommand, IMessageMetadata inputMetadata, IState inputState)
        {
            yield return MessageEnvelopeFactory.CreateEnvelope(_command);
        }
    }
}