using System;
using System.Collections.Generic;
using ProtoBuf;
using System.Linq;

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

        /// <summary>
        /// Metadata with value of various types
        /// </summary>
        [ProtoMember(20)] public Dictionary<String, String> StringMetadata { get; set; }
        [ProtoMember(21)] public Dictionary<String, Int32> Int32Metadata { get; set; }
        [ProtoMember(22)] public Dictionary<String, Int64> Int64Metadata { get; set; }
        [ProtoMember(23)] public Dictionary<String, Guid> GuidMetadata { get; set; }
        // etc.


        /// <summary>
        /// Should not be serialized
        /// </summary>
        public Dictionary<String, Object> Metadata 
        { 
            get
            {
                var dict = new Dictionary<String, Object>();

                foreach (var keyValuePair in StringMetadata) dict.Add(keyValuePair.Key, keyValuePair.Value);
                foreach (var keyValuePair in Int32Metadata) dict.Add(keyValuePair.Key, keyValuePair.Value);
                foreach (var keyValuePair in Int64Metadata) dict.Add(keyValuePair.Key, keyValuePair.Value);
                foreach (var keyValuePair in GuidMetadata) dict.Add(keyValuePair.Key, keyValuePair.Value);

                return dict;
            }
        }

        

        public MessageHeader()
        {
            StringMetadata = new Dictionary<String, String>();
            Int32Metadata = new Dictionary<String, Int32>();
            Int64Metadata = new Dictionary<String, Int64>();
            GuidMetadata = new Dictionary<String, Guid>();
        }

        public MessageHeader(Guid messageTag) : this()
        {
            MessageTag = messageTag;
        }

        public void AddMetadata(String key, String value)
        {
            Metadata.Add(key, value);
        }
    }
}