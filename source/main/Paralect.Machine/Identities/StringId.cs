using System;
using ProtoBuf;

namespace Paralect.Machine.Identities
{
    /// <summary>
    /// Only String are supported for now because Abe uses Strings as identities.
    /// For serialization compatibility, members tags are limited in range of [1, 5] inclusively.
    /// </summary>
    [ProtoContract]
    public abstract class StringId : IIdentity<String>
    {
        /// <summary>
        /// Value of ID
        /// </summary>
        [ProtoMember(1)] 
        public String Value { get; set; }

        /// <summary>
        /// Using hash code of the string id
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            var stringId = obj as StringId;

            if (stringId == null)
                return 1;

            return Value.CompareTo(stringId.Value);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as StringId;

            if (identity != null)
                return identity.Value.Equals(Value);

            return false;
        }

        /// <summary>
        /// Returns a string that represents the current StringId
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
}