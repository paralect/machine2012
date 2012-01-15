using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    public class ReplyResult : IResult
    {
        private readonly ICommand _command;

        public ICommand Command
        {
            get { return _command; }
        }

        public ReplyResult(ICommand command)
        {
            this._command = command;
        }

        public IEnumerable<IMessage> BuildMessages(ICommand command, IAggregateState state)
        {
            yield return _command;
        }
    }
}