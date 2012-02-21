using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    /*public class OperationContext
    {
        public IMessage Message { get; set; }
        public Header MessageHeaders { get; set; }
        public IProcessState State { get; set; }
        public Header StateHeaders { get; set; }

        // .. to be continued
    }*/

    public interface IProcess
    {
        /// <summary>
        /// Execute command 
        /// </summary>
        IEnumerable<IMessage> Execute(ICommand command, ICommandMetadata header, IState state);

        /// <summary>
        /// Notify about event
        /// </summary>
        IEnumerable<IMessage> Notify(IEvent evnt, IEventMetadata metadata, IState state);
    }
}