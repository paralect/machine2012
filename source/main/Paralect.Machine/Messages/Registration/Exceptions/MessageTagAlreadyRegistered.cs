using System;

namespace Paralect.Machine.Messages
{
    public class MessageTagAlreadyRegistered : Exception
    {
        public MessageTagAlreadyRegistered(Guid tag, Type message1type, Type message2type, Exception innerException = null) : 
            base(String.Format("Message tag '{0}' already registered. You are trying to use the same tag for two messages: '{1}' and '{2}'", 
                tag, message1type, message2type), innerException)
        {
        }
    }
}