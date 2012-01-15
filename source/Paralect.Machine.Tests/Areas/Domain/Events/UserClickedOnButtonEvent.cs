using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paralect.Machine.Domain;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Domain.Test.Events
{
    public class UserClickedOnButtonEvent : Event
    {
        public String UserId { get; set; }
        public String ButtonText { get; set; }
    }
}
