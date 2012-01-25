using System;
using ProtoBuf;

namespace Paralect.Machine.Identities
{
    /// <summary>
    /// Only String are supported for now because Abe uses Strings as identities.
    /// For serialization compatibility, members tags are limited in range of [1, 5] inclusively.
    /// </summary>
    [ProtoContract]
    public class GuidId : IIdentity<Guid>
    {
        private Guid _value;

        /// <summary>
        /// Value of ID
        /// </summary>
        String IIdentity.Value
        {
            get { return _value.ToString(); }
        }

        [ProtoMember(1)]
        public Guid Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Factory method
        /// </summary>
        public static GuidId Create(String guidText)
        {
            return new GuidId { Value = Guid.Parse(guidText) };
        }

        /// <summary>
        /// Factory method
        /// </summary>
        public static GuidId Create(Guid guid)
        {
            return new GuidId { Value = guid };
        }

        public static GuidId CreateNew()
        {
            return new GuidId { Value = Guid.NewGuid() };
        }


        /// <summary>
        /// Using hash code of the string id
        /// </summary>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as GuidId;

            if (identity != null)
                return identity.Value.Equals(Value);

            return false;
        }

        /// <summary>
        /// Returns a string that represents the current StringId
        /// </summary>
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}