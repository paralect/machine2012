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

        public IEnumerable<IMessage> BuildMessages(IMessage command, IProcessState state)
        {
            yield return _command;
        }
    }
}