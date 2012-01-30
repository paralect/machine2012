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
        [Test]
        public void simple_pum_para_pum()
        {
            var journalStorage = new InMemoryJournalStorage();

            var context = MachineContext.Create(b => b
                .RegisterMessages(typeof(EnvelopeSerializer_Event), typeof(EnvelopeSerializer_Child_Event))
                .RegisterIdentities(typeof(EnvelopeSerializer_Id))
            );

            var message = new EnvelopeSerializer_Event() { Rate = 0.7, Title = "Muahaha!" };

            context.RunHost(h => h
                .AddNode(new RouterNode(context.ZeromqContext, "inproc://rep", "inproc://pub", "inproc://domain", journalStorage))
                .Execute(token =>
                {
                    using (var input = context.ZeromqContext.Socket(SocketType.PUSH))
                    {
                        input.EstablishConnect("inproc://rep", token);

                        input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                        input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                        input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                        input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));
                        input.SendBinaryEnvelope(context.CreateBinaryEnvelope(message));

                    }                    
                })
            );

            var seq = journalStorage.GetPrivateFieldValue<Int64>("_sequance");

            Assert.That(seq, Is.EqualTo(5));
        }
    }
}