using System;

namespace Paralect.Machine.Identities
{
    public class IdentityDefinition
    {
        public Type Type { get; set; }
        public Guid Tag { get; set; }
        public Boolean Abstract { get; set; }

        public IdentityDefinition(Type type, Guid tag, Boolean @abstract = false)
        {
            Type = type;
            Tag = tag;
            Abstract = @abstract;
        }         
    }
}