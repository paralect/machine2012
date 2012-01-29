using Paralect.Machine.Processes.Utilities;

namespace Paralect.Machine.Processes
{
    public class ProcessFactory
    {
         public TAggregateRoot Create<TAggregateRoot>() 
             where TAggregateRoot : IProcess
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