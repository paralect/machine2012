using System;

namespace Paralect.Machine.Domain
{
    public interface IRepository 
    {
        void Save(MPowerAggregateRoot aggregate);

        /// <summary>
        /// Generic version
        /// </summary>
        TAggregate GetById<TAggregate>(String id)
            where TAggregate : MPowerAggregateRoot;
    }
}