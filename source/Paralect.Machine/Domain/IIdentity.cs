using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Strongly-typed identity class. Essentially just an ID with a 
    /// distinct type. It introduces strong-typing and speeds up development
    /// on larger projects. Idea by Jeremie, implementation approach stolen from Rinat
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        String Value { get; }
    }
}
