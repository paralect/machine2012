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
            if (message == null) throw new ArgumentNullException("message");
            if (messageMetadata == null) throw new ArgumentNullException("messageMetadata");

            if (message is IEvent)
            {
                if (!(messageMetadata is IEventMetadata))
                    throw new Exception("Event metadata must implement IEventMetadata");

                return new EventEnvelope(serializer, (IEventMetadata) messageMetadata, (IEvent) message);
            }

            if (message is ICommand)
            {
                if (!(messageMetadata is ICommandMetadata))
                    throw new Exception("Command metadata must implement ICommandMetadata");

                return new CommandEnvelope(serializer, (ICommandMetadata) messageMetadata, (ICommand) message);
            }

            return new MessageEnvelope(serializer, messageMetadata, message);
        }

        /// <summary>
        /// Creates envelope with default and empty metadata
        /// </summary>
        public static IMessageEnvelope CreateEnvelope(PacketSerializer serializer, IMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            if (message is IEvent)
            {
                return new EventEnvelope(serializer, new EventMetadata(), (IEvent)message);
            }

            if (message is ICommand)
            {
                return new CommandEnvelope(serializer, new CommandMetadata(), (ICommand)message);
            }

            return new MessageEnvelope(serializer, new MessageMetadata(), message);
        }
    }
}