using System;

namespace Paralect.Machine.Messages
{
    public static class EnvelopeFactory
    {
        /// <summary>
        /// Boring code that enforce correct message and metadata pairs for events, commands and ordinary messages
        /// </summary>
        public static IPacketMessageEnvelope CreateEnvelope(PacketSerializer serializer, IMessage message, IMessageMetadata messageMetadata)
        {
            return new PacketMessageEnvelope(serializer, message, messageMetadata);
        }        
        
        /// <summary>
        /// Boring code that enforce correct message and metadata pairs for events, commands and ordinary messages
        /// </summary>
        public static IPacketMessageEnvelope CreateEnvelope(PacketSerializer serializer, byte[] message, IMessageMetadata messageMetadata)
        {
            return new PacketMessageEnvelope(serializer, message, messageMetadata);
        }

        /// <summary>
        /// Creates envelope with default and empty metadata
        /// </summary>
        public static IPacketMessageEnvelope CreateEnvelope(PacketSerializer serializer, IMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            IMessageMetadata metadata;

            if (message is IEvent)
                metadata = new EventMetadata();
            else if (message is ICommand)
                metadata = new CommandMetadata();
            else
                metadata = new MessageMetadata();

            return new PacketMessageEnvelope(serializer, message, metadata);
        }
    }
}