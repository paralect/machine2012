using System;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Only String are supported for now because Abe uses Strings as identities.
    /// </summary>
    public abstract class StringIdentity : IIdentity
    {
        public abstract String Value { get; protected set; }

        public StringIdentity(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Using hash code of the id
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}