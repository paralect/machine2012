using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent eventMessage);
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}