using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization;
using Paralect.Machine.Mongo;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// IdentityFactory owns all information about Identities that can be found in domain's assemlies.
    /// We are finding Identities by EntityTagAttribute. Maybe we need find them by IIdentity instead?..
    /// </summary>
    public class IdentityFactory
    {
        /// <summary>
        /// (Entity Tag --> Entity Type) and (Entity Type --> Entity Tag) maps
        /// </summary>
        private static readonly Dictionary<int, Type> _tagToTypeMap = new Dictionary<int, Type>(100);
        private static readonly Dictionary<Type, int> _typeToTagMap = new Dictionary<Type, int>(100);

        /// <summary>
        /// All registered Identity Types
        /// </summary>
        public IEnumerable<Type> IdentityTypes
        {
            get { return _typeToTagMap.Keys; }
        }

        /// <summary>
        /// Creates IdentityFactory with specified set of identities.
        /// </summary>
        public IdentityFactory(IEnumerable<Type> identityTypes)
        {
            Initialize(identityTypes);
        }

        /// <summary>
        /// Creates IdentityFactory with specified set of identities.
        /// </summary>
        public IdentityFactory(params Type[] identityTypes)
        {
            Initialize(identityTypes);
        }

        /// <summary>
        /// Initializes Factory state. Checks that EntityTags are unique.
        /// </summary>
        private void Initialize(IEnumerable<Type> identityTypes)
        {
            foreach (var type in identityTypes)
            {
                var tagAttribute = GetSingleAttribute<EntityTagAttribute>(type);

                if (tagAttribute == null)
                    continue;

                if (_tagToTypeMap.ContainsKey(tagAttribute.Tag))
                    throw new Exception(String.Format((string)"Tag {0} already registered.", (object)tagAttribute.Tag));

                if (_typeToTagMap.ContainsKey(type))
                    throw new Exception(String.Format((string)"Identity type {0} already registered.", (object)tagAttribute.Tag)); // hope this is just impossible

                _tagToTypeMap[tagAttribute.Tag] = type;
                _typeToTagMap[type] = tagAttribute.Tag;
            }            
        }

        /// <summary>
        /// Returns entity tag by Identity type
        /// </summary>
        public static Int32 GetTag(Type identityType)
        {
            return _typeToTagMap[identityType];
        }

        /// <summary>
        /// Returns Identity Type by enitity tag value
        /// </summary>
        public static Type GetIdentityType(Int32 tag)
        {
            return _tagToTypeMap[tag];
        }


        #region Reflection helpers

        /// <summary>
        /// Returns attribute instance for specified type. Will return default type value if not found or not single.
        /// </summary>
        public static TAttribute GetSingleAttribute<TAttribute>(Type type)
        {
            var identities = type.GetCustomAttributes(typeof(TAttribute), false);

            if (identities.Length != 1)
                return default(TAttribute);

            if (!(identities[0] is TAttribute))
                return default(TAttribute);

            return (TAttribute)identities[0];
        }

        #endregion
    }

}