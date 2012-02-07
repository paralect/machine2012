using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paralect.Machine.Journals;
using Paralect.Machine.Messages;
using Paralect.Machine.Nodes;
using Paralect.Machine.Routers;
using Paralect.Machine.Tests.Areas.Serialization.Fixtures;
using Paralect.Machine.Utilities;
using ZMQ;

namespace Paralect.Machine.Tests.Areas.Domain.Fixtures
{
    [TestFixture]
    public class RouterAndProcessNodesTest
    {
        [Test]
        public void simple_pum_para_pum()
        {
            var journalStorage = new InMemoryJournalStorage();

            var context = MachineContext.Create(b => b
                .RegisterMessages(typeof(EnvelopeSerializer_Event), typeof(EnvelopeSerializer_Child_Event))
                .RegisterIdentities(typeof(EnvelopeSerializer_Id))
                .AddRouter("process", new ProcessRouter())
            );

            var message = new EnvelopeSerializer_Event() { Rate = 0.7, Title = "Muahaha!" };
            var messageMetadata = new EventMetadata() { Receivers = new[] {new EnvelopeSerializer_Id() { Value = "hello" }} };

            context.RunHost(h => h
                .AddNode(new RouterNode(context, "inproc://rep", "inproc://pub", "inproc://domain", journalStorage))
                .AddNode(new ProcessesNode(context, "inproc://domain", "inproc://rep"))
                .SetTimeout(700)
                .Execute(token =>
                {
                    using (var input = context.CreateSocket(SocketType.PUSH))
                    {
                        input.Connect("inproc://rep", token);

                        input.SendPacket(context.CreatePacket(message, messageMetadata));
                        input.SendPacket(context.CreatePacket(message, messageMetadata));
                        input.SendPacket(context.CreatePacket(message, messageMetadata));
                        input.SendPacket(context.CreatePacket(message, messageMetadata));
                        input.SendPacket(context.CreatePacket(message, messageMetadata));
                    }                    
                })
            );

            var seq = journalStorage.GetPrivateFieldValue<Int64>("_sequance");
            var storage = journalStorage.GetPrivateFieldValue<SortedList<Int64, IMessageEnvelope>>("_storage");

            Assert.That(seq, Is.EqualTo(5));
            var firstMessage = (EnvelopeSerializer_Event)storage[1].GetMessage();
            Assert.That(firstMessage == message , Is.False);
            Assert.That(firstMessage.Rate, Is.EqualTo(message.Rate));
            Assert.That(firstMessage.Title, Is.EqualTo(message.Title));

            var result = journalStorage.Load(3, 100);
        }
    }
}