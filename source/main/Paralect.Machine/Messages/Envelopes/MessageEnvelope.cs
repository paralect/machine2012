using System;

namespace Paralect.Machine.Messages
{
    public class MessageEnvelope : IMessageEnvelope, IMessageEnvelopeBinary
    {
        private byte[] _metadataBinary;
        private byte[] _messageBinary;

        private IMessageMetadata _metadata;
        private IMessage _message;

        /// <summary>
        /// Message tag and key-value information
        /// </summary>
        public IMessageMetadata GetMetadata()
        {
            /*
            if (_metadata == null)
            {
                _metadata = null;//GetHeadersCopy();
                _metadataBinary = null;
            }*/

            return _metadata;
        }

        /// <summary>
        /// Actual single message in the envelope
        /// </summary>
        public IMessage GetMessage()
        {
            return _message;
        }

        public byte[] GetMetadataBinary()
        {
            return _metadataBinary;
        }

        public byte[] GetMessageBinary()
        {
            return _messageBinary;
        }


        /// <summary>
        /// Constructs MessageEnvelope
        /// </summary>
        public MessageEnvelope(IMessageMetadata metadata, IMessage message)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            if (message == null)
                throw new ArgumentNullException("message");

//            if (metadata.MessageTag == Guid.Empty)
//                throw new Exception("Message tag was not specified in message header");

            _metadata = metadata;
            _message = message;
        }
    }
}