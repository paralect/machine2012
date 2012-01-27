using System;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Journals.Abstract
{
    public interface IJournalStorage
    {
        Int64 Save(JournalEntry entry);
    }
}