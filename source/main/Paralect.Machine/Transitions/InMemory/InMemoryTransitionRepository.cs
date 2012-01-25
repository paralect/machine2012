using System;
using System.Linq;
using System.Collections.Generic;
using Paralect.Machine.Transitions;

namespace Paralect.Machine.InMemory.Transitions
{
    public class InMemoryTransitionRepository : ITransitionRepository
    {
        private readonly List<Transition> _transitions = new List<Transition>();

        public void SaveTransition(Transition transition)
        {
            _transitions.Add(transition);
        }

        public List<Transition> GetTransitions(string streamId, int fromVersion, int toVersion)
        {
            return _transitions.Where(t =>
                t.Id.StreamId == streamId &&
                t.Id.Version >= fromVersion &&
                t.Id.Version <= toVersion)
                .ToList();
        }

        /// <summary>
        /// Get all transitions ordered ascendantly by Timestamp of transiton
        /// Should be used only for testing and for very simple event replying 
        /// </summary>
        public List<Transition> GetTransitions()
        {
            return _transitions;
        }

        public void RemoveTransition(string streamId, int version)
        {
            _transitions.RemoveAll(t => t.Id.StreamId == streamId && t.Id.Version == version);
        }

        public void RemoveStream(string streamId)
        {
            _transitions.RemoveAll(t => t.Id.StreamId == streamId);
        }

        public void EnsureIndexes()
        {
            throw new NotSupportedException("In Memory Repository does not need indexes.");
        }
    }
}