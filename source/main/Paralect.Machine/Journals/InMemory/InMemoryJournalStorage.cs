using System;
using System.Collections.Generic;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;
using System.Linq;

namespace Paralect.Machine.Journals
{
    public class InMemoryJournalStorage : IJournalStorage
    {
        private readonly SortedList<Int64, IPacketMessageEnvelope> _storage = new SortedList<Int64, IPacketMessageEnvelope>();
        private Int64 _sequance = 0;

        public long Save(IEnumerable<IPacketMessageEnvelope> messageEnvelopes)
        {
            foreach (var envelope in messageEnvelopes)
            {
                _sequance++;

                var metadata = envelope.Metadata;
                metadata.JournalSequence = _sequance;
                _storage.Add(_sequance, envelope);
            }

            return _sequance;
        }

        /// <summary>
        /// StartingFrom is Inclusive. Yes, we are repeating message.
        /// </summary>
        public IList<IPacketMessageEnvelope> Load(long greaterOrEqualThan, int count)
        {
            return _storage
                .OrderBy(pair => pair.Key)
                .Where(pair => pair.Key >= greaterOrEqualThan)
                .Take(count)
                .Select(pair => pair.Value)
                .ToList();
        }
    }
}