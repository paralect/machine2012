using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    public class EmptyResult : IResult
    {
        public IEnumerable<IMessage> BuildMessages(IMessage command, IProcessState state)
        {
            yield break;
        }
    }
}