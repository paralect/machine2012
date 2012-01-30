using System;
using System.Threading;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Journals;
using Paralect.Machine.Messages;
using Paralect.Machine.Nodes;
using Paralect.Machine.Routers;
using Paralect.Machine.Serialization;
using Paralect.Machine.Tests.Areas.Engine;
using Paralect.Machine.Tests.Areas.Serialization.Fixtures;
using Paralect.Machine.Utilities;
using ZMQ;

namespace Paralect.Machine.Tests.Areas.Domain.Fixtures
{
    [TestFixture]
    public class RouterNodeTest
    {
        [Ignore]
        public void simple_pum_para_pum()
        {
            var journalStorage = new InMemoryJournalStorage();

            var context = new Context(2);
            var engine = new Host(new System.Collections.Generic.List<INode>()
            {
                new RouterNode(context, "inproc://rep", "inproc://pub", "inproc://domain", journalStorage)
            });

            using (var token = new CancellationTokenSource())
            {
                var task1 = engine.Start(token.Token);

                
                using (var input = context.Socket(SocketType.PUSH))
                {
                    input.EstablishConnect("inproc://rep", token.Token);

                    input.SendBinaryEnvelope(muhaha());
                    //input.SendBinaryEnvelope(muhaha());
                    //input.SendBinaryEnvelope(muhaha());
                    
                }

                //task1.Wait(100000);

                if (task1.Wait(2000))
                    Console.WriteLine("Done without forced cancelation"); // This line shouldn't be reached
                else
                    Console.WriteLine("\r\nRequesting to cancel...");

                var z = journalStorage;
                token.Cancel();
            }
        }

        private BinaryEnvelope muhaha()
        {
            var messageFactory = new MessageFactory(typeof(EnvelopeSerializer_Event), typeof(EnvelopeSerializer_Child_Event));
            var identityFactory = new IdentityFactory(typeof(EnvelopeSerializer_Id));

            var serializer = new ProtobufSerializer();
            serializer.RegisterMessages(messageFactory.MessageDefinitions);
            serializer.RegisterIdentities(identityFactory.IdentityDefinitions);

            var message1 = new EnvelopeSerializer_Event()
            {
                Rate = 0.7,
                Title = "Muahaha!"
            };

            var envelopeSerializer = new EnvelopeSerializer(serializer, messageFactory.TagToTypeResolver);

            var envelope = new EnvelopeBuilder(messageFactory.TypeToTagResolver)
                .AddMessage(message1)
                .BuildAndSerialize(envelopeSerializer);

            return envelope;
        }
    }
}