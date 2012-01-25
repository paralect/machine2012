using System;

namespace Paralect.Machine.Transitions
{
    public interface ITransitionStorage
    {
        ITransitionStream OpenStream(String streamId, Int32 fromVersion, Int32 toVersion);
        ITransitionStream OpenStream(String streamId);
    }
}
