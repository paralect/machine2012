using System;
using Paralect.Machine.Messages;
using Paralect.Machine.Metadata;

namespace Paralect.Machine.Nodes
{
    [Serializable]
    public sealed class EngineInitializing : ISystemEvent
    {
        public override string ToString()
        {
            return "Engine initializing";
        }

        public IMessageMetadata Metadata
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}