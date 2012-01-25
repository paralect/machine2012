using System;

namespace Paralect.Machine.Messages
{
    public class MessageTypeAlreadyRegistered : Exception
    {
        public MessageTypeAlreadyRegistered(Type messageType, Exception innerException = null) : 
            base(String.Format("Message type {0} already registered. Second attempt to register the same message type detected.", 
                messageType), innerException)
        {
        }
    }
}