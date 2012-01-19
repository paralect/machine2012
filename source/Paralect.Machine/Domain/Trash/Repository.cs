using System;
using System.Linq;
using Paralect.Machine.Domain.EventBus;
using Paralect.Machine.Domain.Utilities;
using Paralect.Machine.Messages;
using Paralect.Machine.Transitions;

namespace Paralect.Machine.Domain
{
    public class Repository : IRepository
    {
        private readonly ITransitionStorage _transitionStorage;
        private readonly IEventBus _eventBus;
        private readonly IDataTypeRegistry _dataTypeRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Repository(ITransitionStorage transitionStorage, IEventBus eventBus, IDataTypeRegistry _dataTypeRegistry)
        {
            _transitionStorage = transitionStorage;
            _eventBus = eventBus;
            this._dataTypeRegistry = _dataTypeRegistry;
        }

        public void Save(MPowerAggregateRoot aggregate)
        {
            if (String.IsNullOrEmpty(aggregate.Id))
                throw new ArgumentException("Aggregate id was not specified.");

            var transition = aggregate.CreateTransition(_dataTypeRegistry);

            using (var stream = _transitionStorage.OpenStream(transition.Id.StreamId))
            {
                stream.Write(transition);
            }

            if (_eventBus != null)
                _eventBus.Publish(transition.Events.Select(e => (IEvent)e.Data).ToList<IEvent>());
        }

        public TAggregate GetById<TAggregate>(String id)
            where TAggregate : MPowerAggregateRoot
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException("Aggregate id was not specified.");

            var stream = _transitionStorage.OpenStream(id);

            var obj = AggregateCreator.CreateAggregateRoot<TAggregate>();
            obj.LoadFromTransitionStream(stream);
            return obj;
        }
    }
}
