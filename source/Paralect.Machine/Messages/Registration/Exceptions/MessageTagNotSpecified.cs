using System;

namespace Paralect.Machine.Messages
{
    public class MessageTagNotSpecified : Exception
    {
        public MessageTagNotSpecified(Type messageType, Exception innerException = null) : 
            base(String.Format(
                "Message '{0}' has no tag specified. You should decorate this message with MessageAttribute. Otherwise Machine cannot properly serialize and decerialize this message.", 
                messageType), innerException)
        {
        }
    }
}