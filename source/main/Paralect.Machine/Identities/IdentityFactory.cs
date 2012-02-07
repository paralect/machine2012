using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Identities
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
        private readonly ConcurrentDictionary<Guid, IdentityDefinition> _tagToTypeMap = new ConcurrentDictionary<Guid, IdentityDefinition>();
        private readonly ConcurrentDictionary<Type, IdentityDefinition> _typeToTagMap = new ConcurrentDictionary<Type, IdentityDefinition>();

        /// <summary>
        /// All registered Identity Types
        /// </summary>
        public IEnumerable<IdentityDefinition> IdentityDefinitions
        {
            get { return _tagToTypeMap.Values; }
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
        /// Initializes Factory state
        /// </summary>
        private void Initialize(IEnumerable<Type> identityTypes)
        {
            foreach (var type in identityTypes)
            {
                var currentType = type;
                var baseType = type.BaseType;

                while (baseType != typeof(object) && baseType != null)
                {
                    ProcessIdentityType(currentType);

                    currentType = baseType;
                    baseType = baseType.BaseType;
                }
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessIdentityType(Type type)
        {
            var tagAttribute = GetSingleAttribute<IdentityAttribute>(type);

            if (tagAttribute == null)
                throw new IdentityTagNotSpecified(type);

            if (_typeToTagMap.ContainsKey(type))
                return;

            if (_tagToTypeMap.ContainsKey(tagAttribute.Tag))
                throw new IdentityTagAlreadyRegistered(tagAttribute.Tag, _tagToTypeMap[tagAttribute.Tag].Type, type);

            var definition = new IdentityDefinition(type, tagAttribute.Tag);

            _tagToTypeMap[tagAttribute.Tag] = definition;
            _typeToTagMap[type] = definition;
        }

        /// <summary>
        /// Returns entity tag by Identity type
        /// </summary>
        public Guid GetTag(Type identityType)
        {
            return _typeToTagMap[identityType].Tag;
        }

        /// <summary>
        /// Returns Identity Type by enitity tag value
        /// </summary>
        public Type GetIdentityType(Guid tag)
        {
            return _tagToTypeMap[tag].Type;
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