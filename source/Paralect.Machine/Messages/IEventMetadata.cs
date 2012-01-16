using System;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public interface IEventMetadata : IMessageMetadata
    {
        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        IIdentity SenderId { get; set; }

        /// <summary>
        /// Version of Aggregate Root, Service or Process at the moment event was emitted.
        /// Emitting party should increment version and next event should have version incremented by one.
        /// Can be used to support commutativity of event handling operations.
        /// </summary>
        Int32 SenderVersion { get; set; }        
    }

    public interface IEventMetadata<TSenderId> : IEventMetadata
    {
        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        new TSenderId SenderId { get; set; }        
    }

    /// <summary>
    /// Strongly typed version of event metadata
    /// </summary>
    public interface IEventMetadata<TMessageId, TSenderId> : IEventMetadata<TSenderId>, IMessageMetadata<TMessageId>
        where TSenderId : IIdentity
        where TMessageId : IIdentity
    {

    }
}