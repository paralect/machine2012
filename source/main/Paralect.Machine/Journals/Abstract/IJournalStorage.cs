﻿using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Journals.Abstract
{
    public interface IJournalStorage
    {
        Int64 Save(IEnumerable<IMessageEnvelope> messageEnvelopes);
        IList<IMessageEnvelope> Load(Int64 greatorOrEqualThan, Int32 count);

    }
}