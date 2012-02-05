using System;
using NUnit.Framework;
using Paralect.Machine.Journals;
using Paralect.Machine.Mongo.Journals;
using Paralect.Machine.Mongo.Tests.Messages;
using Paralect.Machine.Routers;
using Paralect.Machine.Utilities;
using ZMQ;

namespace Paralect.Machine.Mongo.Tests.Fixtures
{
    [TestFixture]
    public class RouterNodeTest
    {
/*        [Ignore("Uses mongodb...")]
        public void mongo_journal_integration_test()
        {
            var server = new MongoJournalServer("mongodb://localhost:27018/test_journal");
            var journalStorage = new MongoJournalStorage(server);

            var context = MachineContext.Create(b => b
                .RegisterMessages(typeof(EnvelopeSerializer_Event), typeof(EnvelopeSerializer_Child_Event))
                .RegisterIdentities(typeof(EnvelopeSerializer_Id))
            );

            var message = new EnvelopeSerializer_Event() { Rate = 0.7, Title = "Muahaha!" };

            context.RunHost(h => h
                .AddNode(new RouterNode(context.ZeromqContext, "inproc://rep", "inproc://pub", "inproc://domain", journalStorage))
                .SetTimeout(5000)
                .Execute(token =>
                {
                    using (var input = context.ZeromqContext.Socket(SocketType.PUSH))
                    {
                        input.EstablishConnect("inproc://rep", token);

                        for (int i = 0; i < 100000; i++)
                        {
                            input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                            input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                            input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                            input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                            input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                        }
                    }
                })
            );

            var seq = server.GetCurrentSequence();
            Console.WriteLine(seq);
            //Assert.That(seq, Is.EqualTo(5));
        }*/
    }
}