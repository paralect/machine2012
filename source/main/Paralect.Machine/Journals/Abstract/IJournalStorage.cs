using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Journals.Abstract
{
    public interface IJournalStorage
    {
        Int64 Save(IEnumerable<IPacketMessageEnvelope> messageEnvelopes);
        IList<IPacketMessageEnvelope> Load(Int64 greatorOrEqualThan, Int32 count);

    }
}