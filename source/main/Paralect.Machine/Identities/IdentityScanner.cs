using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Paralect.Machine.Identities
{
    /// <summary>
    /// Scans assemblies for identity types (those that implements IIdentity)
    /// </summary>
    public class IdentityScanner
    {
        /// <summary>
        /// Finds in specified assemblies all non-abstract types that implements IIdentity
        /// </summary>
        public IEnumerable<Type> ScanAssemblies(params Assembly[] assemblies)
        {
            return GetTypesThatImplements<IIdentity>(assemblies).ToList();
        }

        /// <summary>
        /// Finds in all AppDomain assemblies all non-abstract types that implements IIdentity
        /// </summary>
        public IEnumerable<Type> ScanAllAppDomainAssemblies()
        {
            return GetTypesThatImplements<IIdentity>(AppDomain.CurrentDomain.GetAssemblies()).ToList();
        }


        #region Reflection helpers

        /// <summary>
        /// Finds all non abstract types within specified assemblies that implements TInterface
        /// </summary>
        public static IEnumerable<Type> GetTypesThatImplements<TInterface>(params Assembly[] assemblies)
        {
            var type = typeof(TInterface);
            var types = assemblies.ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && type.IsAbstract == false);

            return types;
        }


        #endregion
    }
}