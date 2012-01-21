using System;

namespace Paralect.Machine.Messages
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class MessageAttribute : Attribute
    {
        public Guid Tag { get; set; }
        public Boolean Abstract { get; set; }

        public MessageAttribute(String guidInTextFormat)
        {
            Tag = Guid.Parse(guidInTextFormat);
            Abstract = false;
        }
    }
}