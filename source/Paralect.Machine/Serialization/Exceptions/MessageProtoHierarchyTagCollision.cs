using System;

namespace Paralect.Machine.Messages
{
    public class MessageProtoHierarchyTagCollision : Exception
    {
        public MessageProtoHierarchyTagCollision(Int32 protoHierarchyTag, Type message1type, Type message2type, Exception innerException = null) : 
            base(String.Format(
                "Messages '{0}' and '{1}' have the same ProtoBuf hierarchy tag defined ({2}). Two ways to resolve this: \r\n" +
                "   1) Try to regenerate Message Tag GUID (by changing MessageAttribute attribute of message you just added). \r\n" +
                "      Probobility of collisions is less than 5% (for 2 millions messages), so you should be lucky next time :) \r\n" +
                "   2) Explicitly specify ProtoHierarchyTag property in MessageAttribute attribute (not recommended!)",
                message1type, message2type, protoHierarchyTag), innerException)
        {
        }
    }
}