using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Paralect.Machine.Nodes
{
    /// <summary>
    /// Engine host that starts, waits or cancells execution of Nodes
    /// </summary>
    public class Host : IDisposable
    {
        /// <summary>
        /// Process that are managed by engine
        /// </summary>
        private readonly ICollection<INode> _processes;

        /// <summary>
        /// This stack tracks disposable objects in order to dispose them on shutdown
        /// </summary>
        private readonly Stack<IDisposable> _disposables = new Stack<IDisposable>();

        /// <summary>
        /// Names of processes
        /// </summary>
        private readonly String[] _processesNames;

        /// <summary>
        /// Constructs EngineHost with specified non-empty collection of processes
        /// </summary>
        public Host(ICollection<INode> processes)
        {
            _processes = processes;

            // At least one process should be registered
            if (_processes.Count == 0)
                throw new InvalidOperationException(String.Format("There were no instances of '{0}' registered", typeof(INode).Name));

            // Build list of processes names
            _processesNames = _processes
                .Select(p => String.Format("{0}({1:X8})", p.GetType().Name, p.GetHashCode()))
                .ToArray();

            // Register disposables
            foreach (var process in _processes)
                _disposables.Push(process);

            // Initialize Processes
            Initialize();
        }

        /// <summary>
        /// Start engine host.
        /// This will start each process.
        /// </summary>
        public Task Start(CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                var watch = Stopwatch.StartNew();

                // Try to start all tasks
                var tasks = _processes
                    .Select(p => Task.Factory.StartNew(() => p.Run(token)))
                    .ToArray();

                // Engine started
                SystemInformer.Notify(new EngineStarted(_processesNames));

                try
                {
                    // Wait for all processes to be either completed or canceled 
                    Task.WaitAll(tasks, token);
                }
                catch (OperationCanceledException)
                {
                    // Do nothing
                }

                // Engine stopped
                SystemInformer.Notify(new EngineStopped(watch.Elapsed));
            });
        }

        /// <summary>
        /// Initialize Engine Host
        /// </summary>
        internal void Initialize()
        {
            // About to initialize
            SystemInformer.Notify(new EngineInitializing());

            // Initialize all processes
            foreach (var process in _processes)
                process.Init();

            // All process initialized
            SystemInformer.Notify(new EngineInitialized());
        }

        /// <summary>
        /// Try to dispose all processes
        /// </summary>
        public void Dispose()
        {
            while (_disposables.Count > 0)
            {
                try
                {
                    _disposables.Pop().Dispose();
                }
                catch
                {
                    // Suppressing all exceptions because we unable 
                    // to handle them correctly when engine is shutdowning  
                }
            }            
        }
    }
}
