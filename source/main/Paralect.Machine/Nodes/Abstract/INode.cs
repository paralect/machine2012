using System;
using System.Threading;
using System.Threading.Tasks;

namespace Paralect.Machine.Nodes
{
    public interface INode : IDisposable
    {
        void Initialize();
//        Task Start(CancellationToken token);
        void Start(CancellationToken token);
    }
}