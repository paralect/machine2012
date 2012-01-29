using System;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Nodes
{
    [Serializable]
    public sealed class EngineStarted : ISystemEvent
    {
        public readonly string[] EngineProcesses;

        public EngineStarted(string[] engineProcesses)
        {
            EngineProcesses = engineProcesses;
        }

        public override string ToString()
        {
            return string.Format("Engine started: {0}", string.Join(",", EngineProcesses));
        }

        public IMessageMetadata Metadata
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}