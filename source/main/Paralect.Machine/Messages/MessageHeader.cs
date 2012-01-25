using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    /// <summary>
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

        [ProtoMember(2)]
        public Dictionary<String, String> Metadata { get; set; }
    }
}