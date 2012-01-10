using System;
using System.Collections.Generic;
using System.Linq;

namespace Paralect.Machine.Domain
{
    public class IdentityFactory
    {
        private static readonly Dictionary<Int32, Type> _tagToTypeMap = new Dictionary<int, Type>(100);
        private static readonly Dictionary<Type, Int32> _typeToTagMap = new Dictionary<Type, int>(100);

        public static void Init()
        {
            var types = GetTypesWith<EntityTagAttribute>(false);

            foreach (var type in types)
            {
                var tagAttribute = GetSingleAttribute<EntityTagAttribute>(type);

                if (tagAttribute == null)
                    continue;

                if (_tagToTypeMap.ContainsKey(tagAttribute.Tag))
                    throw new Exception(String.Format("Tag {0} already registered.", tagAttribute.Tag));

                if (_typeToTagMap.ContainsKey(type))
                    throw new Exception(String.Format("Identity type {0} already registered.", tagAttribute.Tag)); // hope this is just impossible

                _tagToTypeMap[tagAttribute.Tag] = type;
                _typeToTagMap[type] = tagAttribute.Tag;
            }
        }

        public static Int32 GetTag(Type identityType)
        {
            return _typeToTagMap[identityType];
        }

        public static Type GetIdentityType(Int32 tag)
        {
            return _tagToTypeMap[tag];
        }

        public static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit) where TAttribute : System.Attribute
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from type in assembly.GetTypes()
                   where type.IsDefined(typeof(TAttribute), inherit)
                   select type;
        }

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

            return (TAttribute) identities[0];
        }
    }

}