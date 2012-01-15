using System;
using Paralect.Machine.Domain.Utilities;

namespace Paralect.Machine.Domain
{
    public class AggregateFactory
    {
         public TAggregateRoot Create<TAggregateRoot>() 
             where TAggregateRoot : IAggregateRoot
         {
             var aggregate = AggregateCreator.CreateAggregateRoot<TAggregateRoot>();

/*             var aggregateInterface = typeof(TAggregateRoot).GetInterface(typeof(IAggregateRoot<,>).FullName);

             var args = aggregateInterface.GetGenericArguments();
             var aggregateStateType = args[1];
             var state = (IAggregateState) Activator.CreateInstance(aggregateStateType);

             aggregate.Initialize(state);*/

             return aggregate;
         }
    }
}