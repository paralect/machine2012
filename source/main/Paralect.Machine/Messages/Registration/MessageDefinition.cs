using System;

namespace Paralect.Machine.Messages
{
    public class MessageDefinition
    {
        public Type Type { get; set; }
        public Guid Tag { get; set; }
        public Boolean Abstract { get; set; }

        public MessageDefinition(Type type, Guid tag, Boolean @abstract = false)
        {
            Type = type;
            Tag = tag;
            Abstract = @abstract;
        }
    }
}