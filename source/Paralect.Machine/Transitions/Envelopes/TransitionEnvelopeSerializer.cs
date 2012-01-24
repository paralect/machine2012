using System;
using System.Collections.Generic;
using System.IO;
using Paralect.Machine.Messages;
using Paralect.Machine.Serialization;
using ProtoBuf;

namespace Paralect.Machine.Transitions
{
    public class TransitionEnvelopeSerializer
    {
        public TransitionEnvelope Serialize(MessageFactory factory, ProtobufSerializer serializer, Transition transition)
        {
            var guids = new Guid[transition.Events.Count];

            for (int i = 0; i < transition.Events.Count; i++)
            {
                var evnt = transition.Events[i];
                guids[i] = factory.GetMessageTag(evnt.GetType());
            }

            var header = new TransitionEnvelopeDataHeader(guids);

            var memory = new MemoryStream();
            serializer.Model.SerializeWithLengthPrefix(memory, header, typeof(TransitionEnvelopeDataHeader), PrefixStyle.Base128, 0);

            foreach (var evnt in transition.Events)
            {
                serializer.Model.SerializeWithLengthPrefix(memory, evnt, evnt.GetType(), PrefixStyle.Base128, 0);
            }

            // TODO: ToArray() is inefficient because of copy
            return new TransitionEnvelope(null, memory.ToArray());

        }

        public Transition Deserialize(MessageFactory factory, ProtobufSerializer serializer, TransitionEnvelope envelope)
        {
            var messages = new List<IEvent>();
            var memory = new MemoryStream(envelope.Data);

            var header = (TransitionEnvelopeDataHeader) serializer.Model.DeserializeWithLengthPrefix(memory, null, typeof(TransitionEnvelopeDataHeader), PrefixStyle.Base128, 0, null);

            foreach (var messageTag in header.MessageTags)
            {
                var messageType = factory.GetMessageType(messageTag);
                var message = (IEvent) serializer.Model.DeserializeWithLengthPrefix(memory, null, messageType, PrefixStyle.Base128, 0, null);
                messages.Add(message);
            }

            var transition = new Transition(null, DateTime.UtcNow, messages, null);
            return transition;
        }
    }
}