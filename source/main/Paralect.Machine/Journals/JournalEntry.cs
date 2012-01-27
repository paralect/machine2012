using System;
using System.Collections.Generic;
using System.Linq;

namespace Paralect.Machine.Journals
{
    public class JournalEntry
    {
        /// <summary>
        /// Values can be one of the following types: 
        /// Int32, Int64, String, Guid.
        /// All unsupported types rendered to String type by calling ToString()
        /// </summary>
        /// <returns>
        /// Returns dictionary of key-value metadata. Can be null - means that there is no metadata for this entry.
        /// </returns>
        public IDictionary<String, Object> Metadata { get; private set; }

        /// <summary>
        /// Order of parts should be preserved when serializing/deserializing.
        /// </summary>
        public IEnumerable<JournalEntryPart> Parts { get; private set; }

        /// <summary>
        /// Creates Journal entry with specified metadata (can be null) and with list of parts
        /// </summary>
        public JournalEntry(IDictionary<String, Object> metadata, IEnumerable<JournalEntryPart> parts)
        {
            Metadata = metadata;
            Parts = parts;
        }

        /// <summary>
        /// Creates Journal entry with specified metadata (can be null) and with array of parts.
        /// </summary>
        public JournalEntry(IDictionary<string, object> metadata, params JournalEntryPart[] parts)
            : this(metadata, (IEnumerable<JournalEntryPart>) parts)
        {
            // empty
        }
    }

    public class JournalEntryPart
    {
        public String Name { get; set; }
        public byte[] Data { get; set; }
    }
}