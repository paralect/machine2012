using System;
using System.Reflection;

namespace Paralect.Machine.Processes.Utilities
{
    public static class AggregateCreator
    {
        public static IProcess CreateAggregateRoot(Type aggregateRootType)
        {
            if (! typeof(IProcess).IsAssignableFrom(aggregateRootType))
            {
                var msg = string.Format("Specified type {0} is not a subclass of AggregateRoot class.", aggregateRootType.FullName);
                throw new ArgumentOutOfRangeException("aggregateRootType", msg);
            }

            // Flags to search for a public and non public contructor.
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            // Get the constructor that we want to invoke.
            var ctor = aggregateRootType.GetConstructor(flags, null, Type.EmptyTypes, null);

            // If there was no ctor found, throw exception.
            if (ctor == null)
            {
                var message = String.Format("No constructor found on aggregate root type {0} that accepts " +
                                            "no parameters.", aggregateRootType.AssemblyQualifiedName);
                throw new Exception(message);
            }

            // There was a ctor found, so invoke it and return the instance.
            var aggregateRoot = (IProcess) ctor.Invoke(null);

            return aggregateRoot;
        }

        public static T CreateAggregateRoot<T>() where T : IProcess
        {
            return (T) CreateAggregateRoot(typeof (T));
        }
    }}
