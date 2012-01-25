using System;

namespace Paralect.Machine.Messages
{
    public class EnvelopeItem
    {
        public MessageHeader Header { get; set; }
        public IMessage Message { get; set; }

        public EnvelopeItem(MessageHeader header, IMessage message)
        {
            if (header == null)
                throw new ArgumentNullException("header");

            if (message == null)
                throw new ArgumentNullException("message");

            if (header.MessageTag == Guid.Empty)
                throw new Exception("Message tag was not specified in message header");

            if (header.Metadata == null)
                throw new Exception("Metadata is null but should be initialized");

            Header = header;
            Message = message;
        }
    }
}