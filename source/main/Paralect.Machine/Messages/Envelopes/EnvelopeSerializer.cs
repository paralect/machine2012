using System;
using System.Collections.Generic;
using System.IO;
using Paralect.Machine.Serialization;
using ProtoBuf;

namespace Paralect.Machine.Messages
{
    public class EnvelopeSerializer
    {
        public byte[] Serialize(ProtobufSerializer serializer, Envelope transition)
        {
            var memory = new MemoryStream();
            serializer.Model.SerializeWithLengthPrefix(memory, transition.Header, typeof(EnvelopeHeader), PrefixStyle.Base128, 0);

            foreach (var item in transition.Items)
            {
                serializer.Model.SerializeWithLengthPrefix(memory, item.Header, typeof(MessageHeader), PrefixStyle.Base128, 0);
                serializer.Model.SerializeWithLengthPrefix(memory, item.Message, item.Message.GetType(), PrefixStyle.Base128, 0);
            }

            // TODO: ToArray() is inefficient because of copy
            return memory.ToArray();
        }

        public Envelope Deserialize(Func<Guid, Type> tagToTypeResolver , ProtobufSerializer serializer, byte[] bytes)
        {
            var memory = new MemoryStream(bytes);

            var envelope = new Envelope();

            envelope.Header = (EnvelopeHeader) serializer.Model.DeserializeWithLengthPrefix(memory, null, typeof(EnvelopeHeader), PrefixStyle.Base128, 0, null);
            while (true)
            {
                var messageHeader = (MessageHeader) serializer.Model.DeserializeWithLengthPrefix(memory, null, typeof(MessageHeader), PrefixStyle.Base128, 0, null);
                
                if (messageHeader == null) // we are reached the end
                    break;

                var messageType = tagToTypeResolver(messageHeader.MessageTag);
                var message = (IMessage) serializer.Model.DeserializeWithLengthPrefix(memory, null, messageType, PrefixStyle.Base128, 0, null);

                var item = new EnvelopeItem(messageHeader, message);
                envelope.AddItem(item);
            }

            return envelope;
        }
    }
}