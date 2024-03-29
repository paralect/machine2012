using System;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Nodes
{
    [Serializable]
    public sealed class EngineInitialized : ISystemEvent
    {
        public override string ToString()
        {
            return "Engine initialized";
        }

        public IMessageMetadata Metadata
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}