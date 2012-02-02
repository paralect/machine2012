using System;
using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    /// <summary>
    /// Serializable object, that represent state of process
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Apply specified event to transite to another state
        /// </summary>
        void Apply(IEvent events);
    }
}