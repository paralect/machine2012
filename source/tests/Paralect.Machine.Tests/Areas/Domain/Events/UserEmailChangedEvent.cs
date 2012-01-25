using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paralect.Machine.Domain;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Domain.Test.Events
{
    public class UserEmailChangedEvent : Event
    {
        public String UserId { get; set; }
        public String Email { get; set; }

        /// <summary>
        /// Id of Aggregate Root, Service or Process that emits this events.
        /// </summary>
        public IIdentity SenderId { get; set; }

        /// <summary>
        /// Version of Aggregate Root, Service or Process at the moment event was emitted.
        /// Emitting party should increment version and next event should have version incremented by one.
        /// Can be used to support commutativity of event handling operations.
        /// </summary>
        public int SenderVersion { get; set; }
    }
}
