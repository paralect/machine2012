using System;
using System.Collections.Generic;
using Paralect.Machine.Domain.Utilities;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    public class AggregateRoot<TIdentity, TAggregateState> : IAggregateRoot<TIdentity, TAggregateState>
        where TIdentity : IIdentity
        where TAggregateState : IAggregateState<TIdentity>
    {
        IEnumerable<IMessage> IAggregateRoot.Execute(ICommand command, IAggregateState state)
        {
            var dynamicThis = (dynamic) this;
            var result = (IResult) dynamicThis.Handle((dynamic) command, (dynamic) state);
            return result.BuildMessages(command, state);

//            return this.AsDynamic().Handle(command, state);
        }

        public IEnumerable<IMessage> Execute(ICommand<TIdentity> command, TAggregateState state)
        {
            // Redirect to explicit implementation
            return ((IAggregateRoot) this).Execute(command, state);
        }

        public ResultCollection<TIdentity> Apply(params IEvent<TIdentity>[] events)
        {
            return new ResultCollection<TIdentity>().Apply(events);
        }

        public ResultCollection<TIdentity> Reply(params ICommand<TIdentity>[] commands)
        {
            return new ResultCollection<TIdentity>().Reply(commands);
        }

        public ResultCollection<TIdentity> Send(params ICommand<TIdentity>[] commands)
        {
            return new ResultCollection<TIdentity>().Send(commands);
        }
    }
}