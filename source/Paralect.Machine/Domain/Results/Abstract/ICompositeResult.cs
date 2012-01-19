using System.Collections.Generic;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Defines composite interface for <see cref="IResult"/> 
    /// </summary>
    public interface ICompositeResult : IResult
    {
        IEnumerable<IResult> Responses { get; }
    }
}