using System;

namespace Paralect.Machine.Messages
{
    public class MessageDefinition
    {
        public Type Type { get; set; }
        public Guid Tag { get; set; }
        public Int32 ProtoHierarchyTag { get; set; }

        public MessageDefinition(Type type, Guid tag, Int32 protoHierarchyTag)
        {
            Type = type;
            Tag = tag;
            ProtoHierarchyTag = protoHierarchyTag;
        }
    }
}