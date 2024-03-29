using System.Collections.Generic;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Processes
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

        public IEnumerable<IMessageEnvelope> BuildMessages(IMessage inputCommand, IMessageMetadata inputMetadata, IState inputState)
        {
            foreach (var response in _responses)
            {
                var messages = response.BuildMessages(inputCommand, inputMetadata, inputState);

                foreach (var message in messages)
                    yield return message;
            }
        }
    }
}