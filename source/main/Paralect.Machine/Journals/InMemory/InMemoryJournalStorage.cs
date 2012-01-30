using System;
using System.Collections.Generic;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Journals
{
    public class InMemoryJournalStorage : IJournalStorage
    {
        private readonly List<Tuple<Header, BinaryMessageEnvelope>> _storage = new List<Tuple<Header, BinaryMessageEnvelope>>();
        private Int64 _sequance = 0;

        public long Save(IEnumerable<BinaryMessageEnvelope> messageEnvelope)
        {
            foreach (var envelope in messageEnvelope)
            {
                _storage.Add(new Tuple<Header, BinaryMessageEnvelope>(envelope.GetHeader(), envelope));
                _sequance++;
            }

            return _sequance;
        }
    }
}