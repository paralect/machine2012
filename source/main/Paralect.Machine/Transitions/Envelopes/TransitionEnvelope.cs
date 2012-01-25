using System;
using System.Collections.Generic;
using Paralect.Machine.Transitions;

namespace Paralect.Machine.Transitions
{
    public class TransitionEnvelope
    {
        public Dictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// First bytes of data is a TransitionEnvelopeDataHeader
        /// </summary>
        public byte[] Data { get; set; }

        public TransitionEnvelope(Dictionary<string, string> metadata, byte[] data)
        {
            Metadata = metadata;
            Data = data;
        }
    }
}