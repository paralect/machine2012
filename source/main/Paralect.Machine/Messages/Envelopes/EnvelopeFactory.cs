using System;

namespace Paralect.Machine.Messages
{
    public static class EnvelopeFactory
    {
        /// <summary>
        /// Boring code that enforce correct message and metadata pairs for events, commands and ordinary messages
        /// </summary>
        public static IMessageEnvelope CreateEnvelope(PacketSerializer serializer, IMessage message, IMessageMetadata messageMetadata)
        {
            return new MessageEnvelope(serializer, message, messageMetadata);
        }

        /// <summary>
        /// Creates envelope with default and empty metadata
        /// </summary>
        public static IMessageEnvelope CreateEnvelope(PacketSerializer serializer, IMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            IMessageMetadata metadata;

            if (message is IEvent)
                metadata = new EventMetadata();
            else if (message is ICommand)
                metadata = new CommandMetadata();
            else
                metadata = new MessageMetadata();
            
            return new MessageEnvelope(serializer, message, metadata);
        }
    }
}