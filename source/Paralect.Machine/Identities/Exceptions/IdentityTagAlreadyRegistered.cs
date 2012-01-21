using System;

namespace Paralect.Machine.Identities
{
    public class IdentityTagAlreadyRegistered : Exception
    {
        public IdentityTagAlreadyRegistered(Guid tag, Type identity1type, Type identity2type, Exception innerException = null) : 
            base(String.Format("Identity tag '{0}' already registered. You are trying to use the same tag for two identity types: '{1}' and '{2}'", 
                tag, identity1type, identity2type), innerException)
        {
        }
    }
}