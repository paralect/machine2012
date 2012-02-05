using System;
using Paralect.Machine.Serialization;

namespace Paralect.Machine.Messages
{
    public class BinaryMessageEnvelope
    {
        public byte[] Header { get; set; }
        public byte[] Message { get; set; }

        public Header GetHeader()
        {
            var serializer = new ProtobufSerializer();
            var back = serializer.Deserialize<Header>(Header);
            return back;
        }

        public void SetHeader(Header header)
        {
            var serializer = new ProtobufSerializer();
            Header = serializer.Serialize(Header);
        }

        public void MutateHeader(Action<Header> headerMutation)
        {
            var header = GetHeader();
            headerMutation(header);
            SetHeader(header);
        }

    }
}