using System;
using Paralect.Machine.Processes.Trash;

namespace Paralect.Machine.Processes
{
    public interface IRepository 
    {
        void Save(LegacyAggregateRoot aggregate);

        /// <summary>
        /// Generic version
        /// </summary>
        TAggregate GetById<TAggregate>(String id)
            where TAggregate : LegacyAggregateRoot;
    }
}