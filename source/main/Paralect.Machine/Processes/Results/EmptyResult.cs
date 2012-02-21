using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    public class EmptyResult : IResult
    {
        public IEnumerable<IMessageEnvelope> BuildMessages(IMessage inputCommand, IMessageMetadata inputMetadata, IState inputState)
        {
            yield break;
        }
    }
}