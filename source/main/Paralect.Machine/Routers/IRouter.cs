using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Routers
{
    public interface IRouter
    {
        Boolean ShouldRoute(IMessageEnvelope envelope);
        IList<IMessageEnvelope> Route(IList<IMessageEnvelope> envelopes);
    }
}