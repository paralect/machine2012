using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
{
    public static class ResultCollectionExtensions
    {
        public static ResultCollection<TIdentity> Apply<TIdentity>(this ResultCollection<TIdentity> builder, params IEvent<TIdentity>[] events)
            where TIdentity : IIdentity
        {
            foreach (var @event in events)
            {
                builder.AddResponse(new ApplyResult(@event));
            }

            return builder;
        }

        public static ResultCollection<TIdentity> Reply<TIdentity>(this ResultCollection<TIdentity> builder, params ICommand<TIdentity>[] commands)
            where TIdentity : IIdentity
        {
            foreach (var @event in commands)
            {
                builder.AddResponse(new ReplyResult(@event));
            }

            return builder;
        }

        public static ResultCollection<TIdentity> Send<TIdentity>(this ResultCollection<TIdentity> builder, params ICommand<TIdentity>[] commands)
            where TIdentity : IIdentity
        {
            foreach (var @event in commands)
            {
                builder.AddResponse(new ReplyResult(@event));
            }

            return builder;
        }
    }
}