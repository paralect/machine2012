using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    /// <summary>
    /// Represents request to apply event.
    /// </summary>
    public class ApplyResult : IResult
    {
        private readonly IEvent evnt;

        public IEvent Event
        {
            get { return evnt; }
        }

        public ApplyResult(IEvent evnt)
        {
            this.evnt = evnt;
        }

        public IEnumerable<IMessage> BuildMessages(IMessage inputCommand, IMessageMetadata inputMetadata, IState inputState)
        {
            yield return evnt;
        }
    }
}