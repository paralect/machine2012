using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Processes.Utilities;

namespace Paralect.Machine.Processes
{
    public class ProcessStateSpooler
    {
        private IProcessState _state;
        private readonly Header _stateHeaders;

        /// <summary>
        /// Aggregate ID
        /// </summary>
        private String _id;

        /// <summary>
        /// Version of Aggregate Root.
        /// Can be used to support commutativity of events.
        /// </summary>
        private Int32 _version;

        public ProcessStateSpooler(IProcessState initialState, Header stateHeaders)
        {
            _state = initialState;
            _stateHeaders = stateHeaders;

            _id = stateHeaders.GetString("Process-State-Id");
            _version = stateHeaders.GetInt32("Process-State-Version");
        }

        /// <summary>
        /// Replay specified events to restore state of IProcessState.
        /// </summary>
        public IProcessState Spool(IEnumerable<Tuple<IEvent, Header>> events)
        {
            foreach (var evnt in events)
            {
                var id = evnt.Item2.GetString("Process-State-Id");
                var version = evnt.Item2.GetInt32("Process-State-Version");

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