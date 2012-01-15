using System;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Engine.Events
{
    [Serializable]
    public sealed class EngineInitializing : ISystemEvent
    {
        public override string ToString()
        {
            return "Engine initializing";
        }
    }
}