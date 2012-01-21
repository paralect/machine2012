using System;

namespace Paralect.Machine.Identities
{
    public class IdentityTagNotSpecified : Exception
    {
        public IdentityTagNotSpecified(Type identityType, Exception innerException = null) : 
            base(String.Format(
                "Identity '{0}' has no tag specified. You should decorate this identity type with IdentityAttribute. Otherwise Machine cannot properly serialize and decerialize this identity (and this identity's descendances).", 
                identityType), innerException)
        {
        }
    }
}