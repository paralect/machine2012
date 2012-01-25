using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paralect.Machine.Identities;
using Paralect.Machine.Referencies;
using ProtoBuf;

namespace Paralect.Machine.Tests.Referencies
{
/*    [EntityTag(1)]
    public class JobId : StringIdentity
    {
        [ProtoMember(1)]
        public override sealed string Value { get; protected set; }

        public JobId(string value) : base(value) { }
    }

    [EntityTag(1)]
    public class UserId : StringIdentity
    {
        [ProtoMember(1)]
        public override sealed string Value { get; protected set; }

        public UserId(string value) : base(value) { }
    }

    [EntityTag(2)]
    public class JobUserReference : IReference<JobId, UserId>
    {
        public JobId From
        {
            get { throw new NotImplementedException(); }
        }

        public UserId To
        {
            get { throw new NotImplementedException(); }
        }
    }
 * */
}
