using System;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    /// <summary>
    /// Transport envelope of set of messages. 
    /// </summary>
    /// <remarks>
    ///   ------------------------------
    ///   |  Envelope                  |
    ///   |                            |
    ///   |    --------------------    |
    ///   |    |                  |    |
    ///   |    |  Message Header  |    |
    ///   |    |                  |    |
    ///   |    --------------------    |
    ///   |    |                  |    |
    ///   |    |                  |    |
    ///   |    |      Message     |    |
    ///   |    |        #1        |    |
    ///   |    |                  |    |
    ///   |    --------------------    |
    ///   |                            |
    ///   |            ...             |
    ///   |                            |
    ///   |    --------------------    |
    ///   |    |                  |    |
    ///   |    |  Message Header  |    |
    ///   |    |                  |    |
    ///   |    --------------------    |
    ///   |    |                  |    |
    ///   |    |                  |    |
    ///   |    |      Message     |    |
    ///   |    |        #N        |    |
    ///   |    |                  |    |
    ///   |    --------------------    |
    ///   |                            |
    ///   |                            |
    ///   ------------------------------
    /// </remarks>
    public class Envelope
    {
        /// <summary>
        /// Messages in the envelope
        /// </summary>
        private readonly List<EnvelopeItem> _items = new List<EnvelopeItem>();

        /// <summary>
        /// Envelope header
        /// </summary>
        public EnvelopeHeader Header { get; set; }

        /// <summary>
        /// Messages in the envelope
        /// </summary>
        public IEnumerable<EnvelopeItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Number of messages in envelope
        /// </summary>
        public Int32 ItemsCount
        {
            get { return _items.Count;  }
        }

        /// <summary>
        /// Add item to envelope
        /// </summary>
        public void AddItem(EnvelopeItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _items.Add(item);
        }
    }
}