using System;
using ProtoBuf;

namespace Paralect.Machine.Identities
{
    /// <summary>
    /// Strongly-typed identity class. Essentially just an ID with a 
    /// distinct type. It introduces strong-typing and speeds up development
    /// on larger projects. Idea by Jeremie, implementation approach stolen from Rinat
    /// </summary>
    [ProtoContract]
    [ProtoInclude(10, typeof(StringId))]
    //[ProtoInclude(11, typeof(GuidId))]
    public interface IIdentity : IComparable
    {
        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        String Value { get; }
    }

    public interface IIdentity<TType> : IIdentity
    {
        new TType Value { get; set; }
    }
}
