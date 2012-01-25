using System;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Referencies
{
    public interface IReference<out TFrom, out TTo>
        where TFrom : IIdentity 
        where TTo : IIdentity
    {
        TFrom From { get; }
        TTo To { get; }
    }
}