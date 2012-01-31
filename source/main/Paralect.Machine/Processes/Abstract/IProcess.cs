using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    /*public class OperationContext
    {
        public Header Headers { get; set; }
        public IProcessState State { get; set; }

        // .. to be continued
    }*/

    public interface IProcess
    {
        /// <summary>
        /// Execute command 
        /// </summary>
        IEnumerable<IMessage> Execute(ICommand command, Header header, IProcessState state);

        /// <summary>
        /// Notify about event
        /// </summary>
        IEnumerable<IMessage> Notify(IEvent evnt, Header header, IProcessState state);
    }
}