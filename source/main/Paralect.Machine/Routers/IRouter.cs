using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Routers
{
    public interface IRouter
    {
        Boolean ShouldRoute(IPacketMessageEnvelope envelope);
        IList<IPacketMessageEnvelope> Route(IList<IPacketMessageEnvelope> envelopes);
    }
}