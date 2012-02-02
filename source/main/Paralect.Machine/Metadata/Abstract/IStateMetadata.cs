using System;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Metadata
{
    public interface IStateMetadata
    {
        Int32 Version { get; set; }

        IIdentity ProcessId { get; set; }
    }
}