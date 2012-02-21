using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;
using Paralect.Machine.Processes.Utilities;
using Paralect.Machine.Transitions;

namespace Paralect.Machine.Processes.Trash
{
    public abstract class LegacyAggregateRoot : IProcess
    {
        /// <summary>
        /// Unique identifier of Aggregate Root
        /// </summary>
        protected string _id;

        /// <summary>
        /// Aggregate version. Version 0 means that object was just created.
        /// Once object will be saved it version will be >= 1.
        /// </summary>
        private int _version = 0;

        /// <summary>
        /// List of changes (i.e. list of pending events)
        /// </summary>
        private readonly List<IEvent> _changes = new List<IEvent>();

        /// <summary>
        /// Unique identifier of Aggregate Root
        /// </summary>
        public String Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Aggregate version
        /// </summary>
        public int Version
        {
            get { return _version; }
            internal set { _version = value; }
        }

        protected LegacyAggregateRoot() { }

        /// <summary>
        /// Create changeset. Used to persist changes in aggregate
        /// </summary>
        public Transition CreateTransition(IDataTypeRegistry dataTypeRegistry)
        {
            if (String.IsNullOrEmpty(_id))
                throw new Exception(String.Format("ID was not specified for domain object. AggregateRoot [{0}] doesn't have correct ID. Maybe you forgot to set an _id field?", this.GetType().FullName));

            var transitionEvents = new List<TransitionEvent>();
            foreach (var e in _changes)
            {
// Was here                e.Metadata.StoredDate = DateTime.UtcNow;
// Was here                e.Metadata.TypeName = e.GetType().Name;
                transitionEvents.Add(new TransitionEvent(dataTypeRegistry.GetTypeId(e.GetType()), e, null));
            }

            return new Transition(new TransitionId(_id, _version + 1), DateTime.UtcNow, /*transitionEvents*/ null, null);
        }

        /// <summary>
        /// Load aggreagate from history
        /// </summary>
        public void LoadFromTransitionStream(ITransitionStream stream)
        {
            foreach (var transition in stream.Read())
            {
                foreach (var evnt in transition.Events)
                {
                    Apply((IEvent) null /*evnt.Data*/, false);
                }

                _version = transition.Id.Version;
            }
        }

        /// <summary>
        /// Load aggregate from events
        /// </summary>
        public void LoadFromEvents(IEnumerable<IEvent> events, Int32 version = 1)
        {
            foreach (var evnt in events)
            {
                Apply(evnt, false);
            }

            _version = version;            
        }

        /// <summary>
        /// Apply event on aggregate 
        /// </summary>
        public void Apply(IEvent evnt)
        {
            Apply(evnt, true);
        }

        private void Apply(IEvent evnt, bool isNew)
        {
            this.AsDynamic().On(evnt);
            
            if (isNew) 
                _changes.Add(evnt);
        }

        public void Initialize(IMessage message, Header header, IState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Execute command 
        /// </summary>
        public IEnumerable<IMessage> Execute(ICommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notify about event
        /// </summary>
        public IEnumerable<IMessage> Notify(IEvent evnt)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMessage> Execute(ICommand command, IState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notify about event
        /// </summary>
        public IEnumerable<IMessage> Notify(IEvent evnt, IState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Execute command 
        /// </summary>
        public IEnumerable<IMessage> Execute(ICommand command, Header header, IState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notify about event
        /// </summary>
        public IEnumerable<IMessage> Notify(IEvent evnt, Header header, IState state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMessageEnvelope> Execute(ICommand command, ICommandMetadata header, IState state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMessageEnvelope> Notify(IEvent evnt, IEventMetadata metadata, IState state)
        {
            throw new NotImplementedException();
        }
    }
}
