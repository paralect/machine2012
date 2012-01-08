using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paralect.Machine.Domain;

namespace Paralect.Machine.Tests.Identities
{
    public class SchoolId : IIdentity 
    {
        private String _schoolId;
        private String _schoolDistrictId;

        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        public string GetId()
        {
            return null;
        }

        /// <summary>
        /// Unique tag (should be unique within the assembly) to distinguish
        /// between different identities, while deserializing.
        /// </summary>
        public string EntityTag
        {
            get { throw new NotImplementedException(); }
        }
    }
}
