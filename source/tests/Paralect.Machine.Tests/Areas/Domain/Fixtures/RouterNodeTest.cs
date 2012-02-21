using System;
using System.Collections.Generic;
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
        [Ignore("Unable to test router without process node...")]
        public void simple_pum_para_pum()
        {
            var journalStorage = new InMemoryJournalStorage();

            var context = MachineContext.Create(b => b
                .RegisterMessages(typeof(EnvelopeSerializer_Event), typeof(EnvelopeSerializer_Child_Event))
                .RegisterIdentities(typeof(EnvelopeSerializer_Id))
            );

            var message = new EnvelopeSerializer_Event() { Rate = 0.7, Title = "Muahaha!" };

            context.RunHost(h => h
                .AddNode(new RouterNode(context, "inproc://rep", "inproc://pub", "inproc://domain", journalStorage))
                .SetTimeout(4000)
                .Execute(token =>
                {
                    using (var input = context.CreateSocket(SocketType.PUSH))
                    {
                        input.Connect("inproc://rep", token);

                        input.SendPacket(context.CreatePacket(message));
                        input.SendPacket(context.CreatePacket(message));
                        input.SendPacket(context.CreatePacket(message));
                        input.SendPacket(context.CreatePacket(message));
                        input.SendPacket(context.CreatePacket(message));
                    }                    
                })
            );

            var seq = journalStorage.GetPrivateFieldValue<Int64>("_sequance");
            var storage = journalStorage.GetPrivateFieldValue<SortedList<Int64, IMessageEnvelope>>("_storage");

            Assert.That(seq, Is.EqualTo(5));
            var firstMessage = (EnvelopeSerializer_Event)storage[1].Message;
            Assert.That(firstMessage == message , Is.False);
            Assert.That(firstMessage.Rate, Is.EqualTo(message.Rate));
            Assert.That(firstMessage.Title, Is.EqualTo(message.Title));
        }
    }
}