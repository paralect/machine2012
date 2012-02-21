using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Routers
{
    public class ProcessRouter : IRouter
    {
        public Boolean ShouldRoute(IPacketMessageEnvelope envelope)
        {
            var metadata = envelope.Metadata;

            if (metadata.Receivers == null)
                return false;

            return true;
        }

        public IList<IPacketMessageEnvelope> Route(IList<IPacketMessageEnvelope> envelopes)
        {
            var list = new List<IPacketMessageEnvelope>();

            foreach (var envelope in envelopes)
            {
                if (ShouldRoute(envelope))
                    list.Add(envelope);
            }

            return list;
        }
    }
}