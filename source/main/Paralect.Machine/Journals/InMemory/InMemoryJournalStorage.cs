using System;
using System.Collections.Generic;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Journals.InMemory
{
    public class InMemoryJournalStorage : IJournalStorage
    {
        private List<MessageEnvelope> _storage = new List<MessageEnvelope>();
        private Int64 _sequance = 0;

        public long Save(IEnumerable<MessageEnvelope> messageEnvelope)
        {
            foreach (var envelope in messageEnvelope)
            {
                _storage.Add(envelope);
                _sequance++;
            }

            return _sequance;
        }
    }
}