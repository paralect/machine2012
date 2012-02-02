using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Paralect.Machine.Identities;

namespace Paralect.Machine.Tests.Areas.Performance.Fixtures
{
    [TestFixture]
    public class PropertyAccessVsDictionaryAccessTest
    {
        public const int COUNT = 10000000;

        [Ignore]
        public void test()
        {
            List<SimpleClass> list = new List<SimpleClass>();

            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < COUNT; i++)
            {
                list.Add(new SimpleClass()
                {
                    MessageId = new List<Guid>(1) { Guid.Empty },
                    TriggerMessageId = new List<Guid>(1) { Guid.Empty }
                });
            }
            watch.Stop();

            Console.WriteLine("\tAllocation speed: {0}", watch.ElapsedMilliseconds);            
        }        
        
        [Ignore]
        public void test_()
        {
            var z = new SimpleClass()
            {
                MessageId = new List<Guid>(1) { Guid.Empty },
                TriggerMessageId = new List<Guid>(1) { Guid.Empty },
                LamportTimestamp = 55
            };


            Stopwatch watch = Stopwatch.StartNew();
            long sum = 0;
            for (int i = 0; i < COUNT; i++)
            {
                sum = sum + z.LamportTimestamp;
            }
            watch.Stop();

            Console.WriteLine("\nAccess speed: {0}", watch.ElapsedMilliseconds);            
        }

        [Ignore]
        public void test2()
        {
            List<Header> list = new List<Header>();

            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < COUNT; i++)
            {
                var header = new Header();
                header.Set("F", Guid.Empty);
                header.Set("Fasdfasdf", 565);
//                header.Set("LONDFDFDFDFDFDFDFcdasdfasdf", 565);
//                header.Set("LONDFDFDFDFDFDFDFasdddfasdf", 565);
                list.Add(header);
            }
            watch.Stop();

            Console.WriteLine("\tAllocation speed: {0}", watch.ElapsedMilliseconds);            
        }

        [Ignore]
        public void test2_()
        {
            var header = new Header();
            header.Set("Fakldjfhklahdfjka asdkfh", Guid.Empty);
            header.Set("Fasdfasdf alksdhakldfh", 565);
            header.Set("Lamport-Timestamp", (long) 55);

            long sum = 0;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < COUNT; i++)
            {
                sum = sum + header.GetInt64("Lamport-Timestamp");
            }
            watch.Stop();

            Console.WriteLine("\tAccess speed: {0}", watch.ElapsedMilliseconds);            
        }
    }

    public class SimpleClass
    {
        public List<Guid> MessageId { get; set; }
        public List<Guid> TriggerMessageId { get; set; }
        public ICollection<IIdentity> Receivers { get; set; }
        public long LamportTimestamp { get; set; }
        public DateTime DeliverOnUtc { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}