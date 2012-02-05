using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Processes.Utilities;

namespace Paralect.Machine.Processes
{
    public class StateSpooler
    {
        private readonly IState _state;

        /// <summary>
        /// Aggregate ID
        /// </summary>
        private readonly IIdentity _id;

        /// <summary>
        /// Version of Aggregate Root.
        /// Can be used to support commutativity of events.
        /// </summary>
        private readonly Int32 _version;

        public StateSpooler(IState initialState, IStateMetadata stateMetadata)
        {
            _state = initialState;
            _id = stateMetadata.ProcessId;
            _version = stateMetadata.Version;
        }

        /// <summary>
        /// Replay specified events to restore state of IProcessState.
        /// </summary>
        public IState Spool(IEnumerable<Tuple<IEvent, IEventMetadata>> events)
        {
            foreach (var evnt in events)
            {
                var id = evnt.Item2.SenderId;
                var version = evnt.Item2.SenderVersion;

                if (_id == null)
                    throw new NullReferenceException("Id of state cannot be null");

                if (id != _id)
                    throw new Exception("State restoration failed because of different Process ID in the events");

                if (version <= _version)
                    throw new Exception("State restoration failed because of wrong version sequence.");

                _state.Apply(evnt.Item1);
            }

            return _state;
        }
    }
}