using System;

namespace Paralect.Machine.Messages
{
    public static class EnvelopeFactory
    {
        /// <summary>
        /// Boring code that enforce correct message and metadata pairs for events, commands and ordinary messages
        /// </summary>
        public static IMessageEnvelope CreateEnvelope(IMessage message, IMessageMetadata messageMetadata)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (messageMetadata == null) throw new ArgumentNullException("messageMetadata");

            if (message is IEvent)
            {
                if (!(messageMetadata is IEventMetadata))
                    throw new Exception("Event metadata must implement IEventMetadata");

                return new EventEnvelope((IEventMetadata) messageMetadata, (IEvent) message);
            }

            if (message is ICommand)
            {
                if (!(messageMetadata is ICommandMetadata))
                    throw new Exception("Command metadata must implement ICommandMetadata");

                return new CommandEnvelope((ICommandMetadata) messageMetadata, (ICommand) message);
            }

            return new MessageEnvelope(messageMetadata, message);
        }

        /// <summary>
        /// Creates envelope with default and empty metadata
        /// </summary>
        public static IMessageEnvelope CreateEnvelope(IMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            if (message is IEvent)
            {
                return new EventEnvelope(new EventMetadata(), (IEvent)message);
            }

            if (message is ICommand)
            {
                return new CommandEnvelope(new CommandMetadata(), (ICommand)message);
            }

            return new MessageEnvelope(new MessageMetadata(), message);
        }
    }
}