using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Messages
{
    public class MessageScanner
    {
        /// <summary>
        /// Finds in specified assemblies all non-abstract types that implements IMessage
        /// </summary>
        public IEnumerable<Type> ScanAssemblies(params Assembly[] assemblies)
        {
            return GetTypesThatImplements<IMessage>(assemblies).ToList();
        }

        /// <summary>
        /// Finds in all AppDomain assemblies all non-abstract types that implements IIdentity
        /// </summary>
        public IEnumerable<Type> ScanAllAppDomainAssemblies()
        {
            return GetTypesThatImplements<IMessage>(AppDomain.CurrentDomain.GetAssemblies()).ToList();
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