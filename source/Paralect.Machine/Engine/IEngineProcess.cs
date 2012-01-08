using System;
using System.Threading;
using System.Threading.Tasks;

namespace Paralect.Machine.Engine
{
    public interface IEngineProcess : IDisposable
    {
        void Initialize();
        Task Start(CancellationToken token);
    }
}