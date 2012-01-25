using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Transitions
{
    /// <summary>
    /// Transition - is a way to group a number of modifications (events) 
    /// for **one** Stream (usually Aggregate Root) in one atomic package, 
    /// that can be either canceled or persisted by Event Store.
    /// </summary>    
    public class Transition
    {
        /// <summary>
        /// Transition ID (StreamId, Version)
        /// </summary>
        public TransitionId Id { get; private set; }

        /// <summary>
        /// DateTime when transition was saved to the Store
        /// (or more accurately - current datetime that was set to Transition _before_ storing it to the Store)
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Events in commit
        /// </summary>
        public List<IEvent> Events { get; private set; }

        /// <summary>
        /// Metadata of commit
        /// </summary>
        public Dictionary<String, Object> Metadata { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Transition(TransitionId transitionId, DateTime timestamp, List<IEvent> events, Dictionary<string, object> metadata)
        {
            Id = transitionId;
            Events = events;
            Metadata = metadata ?? new Dictionary<String, Object>();
            Timestamp = timestamp;
        }
    }
}
