using System;
using System.Collections.Generic;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Journals.InMemory
{
    public class InMemoryJournalStorage// : IJournalStorage
    {
        private readonly List<BinaryMessageEnvelope> _storage = new List<BinaryMessageEnvelope>();
        private Int64 _sequance = 0;

        public long Save(MessageHeader header, IEnumerable<BinaryMessageEnvelope> messageEnvelope)
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