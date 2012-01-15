using System;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface ICommand : IMessage
    {
        /// <summary>
        /// ID of Aggregate Root, Service or Process this command addressed to.
        /// </summary>
        IIdentity ReceiverId { get; set; }
    }

    public interface ICommand<TIdentity> : ICommand 
        where TIdentity : IIdentity
    {
        new TIdentity ReceiverId { get; set; }
    }

    public abstract class Command<TIdentity> : ICommand<TIdentity>
        where TIdentity : IIdentity
    {
        /// <summary>
        /// ID of Aggregate Root, Service or Process this command addressed to.
        /// </summary>
        IIdentity ICommand.ReceiverId 
        {
            get { return ReceiverId;  }
            set { ReceiverId = (TIdentity) value; }
        }

        public TIdentity ReceiverId { get; set; }
    }
}