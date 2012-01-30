using System;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface ICommand : IMessage
    {
    }

    public interface ICommand<TIdentity> : ICommand  where TIdentity : IIdentity
    {
    }
}