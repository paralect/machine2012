using System.Collections.Generic;
using Paralect.Machine.Engine.Events;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Domain
{
    public class ResultCollection<TIdentity> : ICompositeResult
        where TIdentity : IIdentity
    {
        private readonly List<IResult> _responses = new List<IResult>();

        public IEnumerable<IResult> Responses
        {
            get { return _responses; }
        }

        public void AddResponse(IResult result)
        {
            _responses.Add(result);
        }

        public IEnumerable<IMessage> BuildMessages(ICommand command, IAggregateState state)
        {
            foreach (var response in _responses)
            {
                var messages = response.BuildMessages(command, state);

                foreach (var message in messages)
                    yield return message;
            }
        }
    }
}