using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    /// <summary>
    /// Machine Process is a single-threaded, operation-oriented reactive message handler.
    /// 
    /// Rules and desires on implementation of your Process:
    ///     1) I/O operations are forbidden to be used in Process.
    ///     2) Generally, any blocking operations are forbidden.
    ///     3) Idempotence of operation is a MUST, all operations have to be idempotent. If you cannot
    ///        make your operation idempotent - think more about it or ask for help.
    ///          3.1) As consequence, there is no time flow for single operation. Each operation 
    ///               lives in a clear NOW. Simply speaking time is stopped during one operation. 
    ///               Current time should be somehow derived from the message you are now handling.
    ///               Subsequent handling of the same message should results in the same NOW time.
    ///               DateTime.Now (or similar functions) shouldn't be used in the Process.
    ///     4) Full ACID 2.0. While not required for our current implementation, still consider 
    ///        the following rules as they are required for more robust and more highly scalable applications:
    ///          4.1) Think and try to make your operations associative
    ///          4.2) Think and try to make your operations commutative
    ///     5) Any long-running computation (that takes more than 10 ms) or any operation that requires blocking 
    ///        of the Process thread (for example database or file system access), should be breaked into three phases:
    ///          Phase 1: Request for long or blocking operations by sending a message.
    ///          Phase 2: Handle message by some external handler, and send result of computation back to the Process.
    ///          Phase 3: Handle result.
    /// </summary>
    public class Process<TId, TProcessState> : IProcess<TId, TProcessState>
        where TId : IIdentity
        where TProcessState : IProcessState<TId>
    {
        IEnumerable<IMessage> IProcess.Execute(ICommand command, IProcessState state)
        {
            var dynamicThis = (dynamic) this;
            var result = (IResult) dynamicThis.Handle((dynamic) command, (dynamic) state);
            return result.BuildMessages(command, state);

//            return this.AsDynamic().Handle(command, state);
        }

        /// <summary>
        /// Notify about event
        /// </summary>
        public IEnumerable<IMessage> Notify(IEvent evnt, IProcessState state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMessage> Execute(ICommand<TId> command, TProcessState state)
        {
            // Redirect to explicit implementation
            return ((IProcess) this).Execute(command, state);
        }

        public ResultCollection<TId> Apply(params IEvent<TId>[] events)
        {
            return new ResultCollection<TId>().Apply(events);
        }

        public ResultCollection<TId> Reply(params ICommand<TId>[] commands)
        {
            return new ResultCollection<TId>().Reply(commands);
        }

        public ResultCollection<TId> Send(params ICommand<TId>[] commands)
        {
            return new ResultCollection<TId>().Send(commands);
        }


        /*
         * Possible variants:
         * return Subscribe(25).On<DeveloperChanged>();
         * return Subscribe<DeveloperChanged>().From(25);
         * return Subscribe<DeveloperChanged>().Of(25);
         * return Subscribe<DeveloperChanged>(e => 25);  // don't like...
         */
        public ResultCollection<TId> Subscribe<TProcessId, TProcessEvent>(TProcessId id)
            where TProcessEvent : IEvent<TProcessId>
        {
            return new ResultCollection<TId>().Send(null);
        }        
        
        public ResultCollection<TId> Unsubscribe<TEvent>(IIdentity id)
        {
            return new ResultCollection<TId>().Send(null);
        }        

        /* Schedule? */
        public ResultCollection<TId> StartTimer(Int32 afterSeconds, String timerName, Boolean oneTimeOnly = true)
        {
            return new ResultCollection<TId>().Send(null);
        }

        /* Unschedule? */
        public ResultCollection<TId> StopTimer(String timerName)
        {
            return new ResultCollection<TId>().Send(null);
        }

        public ResultCollection<TId> Forward()
        {
            return new ResultCollection<TId>().Send(null);
        } 

        public IResult Empty()
        {
            return new EmptyResult();
        }
    }
}