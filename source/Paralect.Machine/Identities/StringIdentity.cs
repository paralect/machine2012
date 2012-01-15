using System;
using ProtoBuf;

namespace Paralect.Machine.Identities
{
    /// <summary>
    /// Only String are supported for now because Abe uses Strings as identities.
    /// For serialization compatibility, members tags are limited in range of [1, 5] inclusively.
    /// </summary>
    [ProtoContract]
    public abstract class StringIdentity : IIdentity
    {
        [ProtoMember(1)]
        public String Value { get; set; }

        /// <summary>
        /// Using hash code of the string id
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}