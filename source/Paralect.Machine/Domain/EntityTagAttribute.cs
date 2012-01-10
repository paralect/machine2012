using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paralect.Machine.Domain
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class EntityTagAttribute : Attribute
    {
        public Int32 Tag { get; set; }

        public EntityTagAttribute(int tag)
        {
            Tag = tag;
        }
    }
}
