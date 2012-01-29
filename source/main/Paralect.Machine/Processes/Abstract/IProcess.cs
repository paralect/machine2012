using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    public class OperationContext
    {
        public Header Headers { get; set; }
        public IProcessState State { get; set; }

        // .. to be continued
    }

    public interface IProcess
    {
        //void Initialize(OperationContext context);

        /// <summary>
        /// Execute command 
        /// </summary>
        IEnumerable<IMessage> Execute(ICommand command, IProcessState state);

        /// <summary>
        /// Notify about event
        /// </summary>
        IEnumerable<IMessage> Notify(IEvent evnt, IProcessState state);
    }


    public interface IProcess<TIdentity, TAggregateState> : IProcess
        where TIdentity : IIdentity
        where TAggregateState : IProcessState<TIdentity>
    {
        /// <summary>
        /// Execute strongly-typed command 
        /// </summary>
        IEnumerable<IMessage> Execute(ICommand<TIdentity> command, TAggregateState state);
    }
}