using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    public class EmptyResult : IResult
    {
        public IEnumerable<IMessage> BuildMessages(ICommand command, IAggregateState state)
        {
            yield break;
        }
    }
}