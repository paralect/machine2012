using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Operation result of command handling. 
    /// </summary>
    public interface IResult
    {
        IEnumerable<IMessage> BuildMessages(ICommand command, IAggregateState state);
    }
}