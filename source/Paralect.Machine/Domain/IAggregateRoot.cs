using System.Collections;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    public interface IAggregateRoot
    {
        IEnumerable<IMessage> Execute(ICommand command, IAggregateState state);
    }


    public interface IAggregateRoot<TIdentity, TAggregateState> : IAggregateRoot
        where TIdentity : IIdentity
        where TAggregateState : IAggregateState<TIdentity>
    {
        IEnumerable<IMessage> Execute(ICommand<TIdentity> command, TAggregateState state);
    }
}