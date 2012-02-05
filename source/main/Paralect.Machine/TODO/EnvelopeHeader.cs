using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    [ProtoContract]
    public class EnvelopeHeader
    {
        private Dictionary<string, string> _metadata = new Dictionary<string, string>();

        [ProtoMember(1)]
        public Dictionary<String, String> Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        public static EnvelopeHeader Empty
        {
            get { return new EnvelopeHeader(); }
        }

    }
}