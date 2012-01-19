using System;

namespace Paralect.Machine.Messages
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class MessageAttribute : Attribute
    {
        public Guid Tag { get; set; }
        public Int32 ProtoHierarchyTag { get; set; }

        public MessageAttribute(String guidInTextFormat)
        {
            Tag = Guid.Parse(guidInTextFormat);

            var guid = Tag;
            var hashcode = guid.GetHashCode();
            var tag = hashcode;

            if (tag <= 0)
                tag = -tag;

            const int mask = 0x00ffffff;
            tag = tag & mask;

            ProtoHierarchyTag = tag;
        }
    }
}