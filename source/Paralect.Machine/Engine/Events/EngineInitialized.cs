using System;

namespace Paralect.Machine.Engine.Events
{
    [Serializable]
    public sealed class EngineInitialized : ISystemEvent
    {
        public override string ToString()
        {
            return "Engine initialized";
        }
    }
}