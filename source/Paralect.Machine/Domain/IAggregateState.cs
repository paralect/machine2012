using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Serializable object, that represent state of aggregate
    /// </summary>
    public interface IAggregateState
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
        /// Replay specified events to restore state of IAggregateState
        /// </summary>
        void Replay(IEnumerable<IEvent> events);
        
    }

    /// <summary>
    /// Strongly typed serializable object, that represent state of aggregate
    /// </summary>
    public interface IAggregateState<TIdentity> : IAggregateState
        where TIdentity : IIdentity
    {
        /// <summary>
        /// Aggregate ID
        /// </summary>
        new TIdentity Id { get; }

        /// <summary>
        /// Replay specified events to restore state of IAggregateState
        /// </summary>
        void Replay(IEnumerable<IEvent<TIdentity>> events);

        void Apply(IEnumerable<IEvent<TIdentity>> events);
    }
}