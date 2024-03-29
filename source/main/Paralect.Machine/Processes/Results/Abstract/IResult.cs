﻿using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    /// <summary>
    /// Operation result of command handling. 
    /// </summary>
    public interface IResult
    {
        IEnumerable<IMessageEnvelope> BuildMessages(IMessage inputCommand, IMessageMetadata inputMetadata, IState inputState);
    }
}