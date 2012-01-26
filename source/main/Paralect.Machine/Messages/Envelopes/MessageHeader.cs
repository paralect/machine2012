using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    /// <summary>
    /// 
    /// Individual message header. 
    /// 
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
    ///
    /// </summary>
    [ProtoContract]
    public class MessageHeader
    {
        [ProtoMember(1)]
        public Guid MessageTag { get; set; }

        private Dictionary<string, string> _metadata = new Dictionary<string, string>();

        [ProtoMember(2)]
        public Dictionary<String, String> Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        public MessageHeader()
        {
        }

        public MessageHeader(Guid messageTag)
        {
            MessageTag = messageTag;
        }

        public void AddMetadata(String key, String value)
        {
            _metadata.Add(key, value);
        }
    }
}