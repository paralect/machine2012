using System;

namespace Paralect.Machine.Identities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class IdentityAttribute : Attribute
    {
        public Guid Tag { get; set; }
        public Boolean Abstract { get; set; }

        public IdentityAttribute(String guidInTextFormat)
        {
            Tag = Guid.Parse(guidInTextFormat);
            Abstract = false;
        }
    }
}
