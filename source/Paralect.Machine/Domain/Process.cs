using System;
using System.Collections.Generic;
using Paralect.Machine.Domain.Utilities;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
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