using System;

namespace Paralect.Machine.Messages
{
    public class MessageEnvelope
    {
        /// <summary>
        /// Message tag and key-value information
        /// </summary>
        public MessageHeader Header { get; set; }

        /// <summary>
        /// Actual single message in the envelope
        /// </summary>
        public IMessage Message { get; set; }

        /// <summary>
        /// Constructs MessageEnvelope
        /// </summary>
        public MessageEnvelope(MessageHeader header, IMessage message)
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