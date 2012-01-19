using System;
using System.Collections.Generic;

namespace Paralect.Machine.Transitions
{
    public interface ITransitionStream : IDisposable
    {
        IEnumerable<Transition> Read();
        void Write(Transition transition);
    }
}