using System;
using Paralect.Machine.Messages;
using Paralect.Machine.Metadata;

namespace Paralect.Machine.Nodes
{
    [Serializable]
    public sealed class EngineStopped : ISystemEvent
    {
        public TimeSpan Elapsed { get; private set; }

        public EngineStopped(TimeSpan elapsed)
        {
            Elapsed = elapsed;
        }

        public override string ToString()
        {
            return string.Format("Engine Stopped after {0} mins", Math.Round(Elapsed.TotalMinutes, 2));
        }

        public IMessageMetadata Metadata
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

}
