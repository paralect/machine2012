using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Only String are supported for now because Abe uses Strings as identities.
    /// </summary>
    public abstract class StringIdentity : IIdentity
    {
        public abstract String Value { get; protected set; }

        protected StringIdentity(string value)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Value = value;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Using hash code of the string id
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}