using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Processes.Utilities;

namespace Paralect.Machine.Processes
{
    /// <summary>
    /// Strongly-typed aggregate state.
    /// </summary>
    /// <typeparam name="TId">Identity of Aggregate and Aggregate State</typeparam>
    public class ProcessState<TId> : IProcessState where TId : class, IIdentity
    {
        /// <summary>
        /// Replay specified events to restore state of IAggregateState.
        /// Explicit implementation to prevent easy access from non-infrastructural code. In most cases
        /// strongly-typed version should be used.
        /// </summary>
        void IProcessState.Apply(IEvent evnt)
        {
            this.AsDynamic().When(evnt);
        }

        /// <summary>
        /// Apply specified events to restore state of IAggregateState.
        /// Strongly-typed version.
        /// </summary>
        public void Apply(IEvent<TId> evnt)
        {
            // Redirect to explicit Replay
            ((IProcessState)this).Apply(evnt);
        }
    }
}