using System;

namespace Paralect.Machine.Identities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class EntityTagAttribute : Attribute
    {
        public Guid Tag { get; set; }

        public EntityTagAttribute(String guidInTextFormat)
        {
            Tag = Guid.Parse(guidInTextFormat);
        }
    }
}
