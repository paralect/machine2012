using System;
using System.Collections.Generic;
using Paralect.Machine.Domain.Utilities;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Strongly-typed aggregate state.
    /// </summary>
    /// <typeparam name="TIdentity">Identity of Aggregate and Aggregate State</typeparam>
    public class AggregateState<TIdentity> : IAggregateState<TIdentity>
        where TIdentity : class, IIdentity
    {
        /// <summary>
        /// Aggregate ID
        /// </summary>
        public TIdentity Id { get; protected set; }

        /// <summary>
        /// Aggregate ID
        /// </summary>
        IIdentity IAggregateState.Id { get { return Id; } }

        /// <summary>
        /// Version of Aggregate Root.
        /// Can be used to support commutativity of events.
        /// </summary>
        public Int32 Version { get; protected set; }

        /// <summary>
        /// Replay specified events to restore state of IAggregateState.
        /// Explicit implementation to prevent easy access from non-infrastructural code. In most cases
        /// strongly-typed version should be used.
        /// </summary>
        void IAggregateState.Apply(IEnumerable<IEvent> events)
        {
            IEvent last = null;

            foreach (var evnt in events)
            {
                if (Id == null)
                    Id = (TIdentity) evnt.Metadata.SenderId;
                else if (!Id.Equals(evnt.Metadata.SenderId))
                    throw new Exception("State restoration failed because of different Aggregate ID in the events.");

                this.AsDynamic().When(evnt);
                
                last = evnt;
            }

            // Set version of state to the version of latest event.
            // Version will be zero, if no events supplied
            Version = last == null ? 0 : last.Metadata.SenderVersion;
        }

        /// <summary>
        /// Apply specified events to restore state of IAggregateState.
        /// Strongly-typed version.
        /// </summary>
        public void Apply(IEnumerable<IEvent<TIdentity>> events)
        {
            // Redirect to explicit Replay
            ((IAggregateState)this).Apply(events);
        }

        public void Apply(params IEvent<TIdentity>[] events)
        {
            // Redirect to explicit Replay
            ((IAggregateState)this).Apply(events);
        }
    }
}