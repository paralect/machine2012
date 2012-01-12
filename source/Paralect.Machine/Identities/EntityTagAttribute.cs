using System;

namespace Paralect.Machine.Identities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class EntityTagAttribute : Attribute
    {
        public Int32 Tag { get; set; }

        public EntityTagAttribute(int tag)
        {
            Tag = tag;
        }
    }
}
