using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Routers
{
    public class ProcessRouter : IRouter
    {
        public Boolean ShouldRoute(IMessageEnvelope envelope)
        {
            var metadata = envelope.GetMetadata();

            if (metadata.Receivers == null)
                return false;

            return true;
        }

        public IList<IMessageEnvelope> Route(IList<IMessageEnvelope> envelopes)
        {
            var list = new List<IMessageEnvelope>();

            foreach (var envelope in envelopes)
            {
                if (ShouldRoute(envelope))
                    list.Add(envelope);
            }

            return list;
        }
    }
}