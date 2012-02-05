using System;
using Paralect.Machine.Messages;
using Paralect.Machine.Metadata;

namespace Paralect.Machine.Envelopes
{
    public class MessageEnvelope : IMessageEnvelope
    {
        public Byte[] MetadataBinary { get; set; }
        public Byte[] MessageBinary { get; set; }

        /// <summary>
        /// Message tag and key-value information
        /// </summary>
        public IMessageMetadata Metadata
        {
            get { return null; }
        }

        /// <summary>
        /// Actual single message in the envelope
        /// </summary>
        public IMessage Message { get; set; }



        /// <summary>
        /// Constructs MessageEnvelope
        /// </summary>
        public MessageEnvelope(IMessageMetadata metadata, IMessage message)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            if (message == null)
                throw new ArgumentNullException("message");

/*
            if (metadata.MessageTag == Guid.Empty)
                throw new Exception("Message tag was not specified in message header");

            if (metadata.Metadata == null)
                throw new Exception("Metadata is null but should be initialized");
*/

            //Metadata = metadata;
            Message = message;
        }
    }
}