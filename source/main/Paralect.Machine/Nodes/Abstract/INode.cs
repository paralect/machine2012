using System;
using System.Threading;
using System.Threading.Tasks;

namespace Paralect.Machine.Nodes
{
    public interface INode : IDisposable
    {
        void Init();
        void Run(CancellationToken token);
    }
}