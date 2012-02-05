using System;
using System.Collections.Generic;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Journals
{
    public class InMemoryJournalStorage : IJournalStorage
    {
        private readonly List<IMessageEnvelope> _storage = new List<IMessageEnvelope>();
        private Int64 _sequance = 0;

        public long Save(IEnumerable<IMessageEnvelope> messageEnvelopes)
        {
            foreach (var envelope in messageEnvelopes)
            {
                _storage.Add(envelope);
                _sequance++;
            }

            return _sequance;
        }
    }
}