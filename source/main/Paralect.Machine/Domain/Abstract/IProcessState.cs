using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Serializable object, that represent state of process
    /// </summary>
    public interface IProcessState
    {
        /// <summary>
        /// Aggregate ID
        /// </summary>
        IIdentity Id { get; }

        /// <summary>
        /// Version of Aggregate Root.
        /// Can be used to support commutativity of events.
        /// </summary>
        Int32 Version { get; }

        /// <summary>
        /// Apply specified events to restore state of IAggregateState
        /// </summary>
        void Apply(IEnumerable<IEvent> events);
        
    }

    /// <summary>
    /// Strongly typed serializable object, that represent state of process
    /// </summary>
    public interface IProcessState<TIdentity> : IProcessState
        where TIdentity : IIdentity
    {
        /// <summary>
        /// Aggregate ID
        /// </summary>
        new TIdentity Id { get; }

        /// <summary>
        /// Apply specified events to restore state of IProcessState
        /// </summary>
        void Apply(IEnumerable<IEvent<TIdentity>> events);
    }
}