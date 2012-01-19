using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Paralect.Machine.Tests.Areas.Performance.Fixtures
{
    [TestFixture]
    public class TagFromGuidTest
    {
        /// <summary>
        /// Results:
        /// In 2000000 iterations, 114443 duplicated tags. Percent 0.0572215
        /// In 2000000 iterations, 114735 duplicated tags. Percent 0.0573675
        /// In 2000000 iterations, 114323 duplicated tags. Percent 0.0571615
        /// 
        /// In 1000000 iterations, 29308 duplicated tags. Percent 0.029308
        /// In 1000000 iterations, 29263 duplicated tags. Percent 0.029263
        /// In 1000000 iterations, 29394 duplicated tags. Percent 0.029394
        /// 
        /// In 10000 iterations, 2 duplicated tags. Percent 0.0002
        /// In 10000 iterations, 4 duplicated tags. Percent 0.0004
        /// In 10000 iterations, 1 duplicated tags. Percent 0.0001
        /// </summary>
        [Ignore]
        public void test_probability_of_collisions_in_tag_produced_by_taking_3_lowest_bytes_from_hashcode_of_guid()
        {
            var dic = new Dictionary<Int32, Guid>();
            var dup = new List<Int32>();

            const int iterations = 1000000;
            var duplicates = 0;

            for (int i = 0; i < iterations; i++)
            {
                if (i % 100000 == 0)
                    Console.WriteLine(".");

                var guid = Guid.NewGuid();
                var hashcode = guid.GetHashCode();
                var tag = hashcode;
                
                if (tag <= 0)
                    tag = -tag;

                const int mask = 0x00ffffff;


                tag = tag & mask;

                try
                {
                    dic.Add(tag, guid);
                }
                catch(Exception ex)
                {
                    var originalGuid = dic[tag];
                    duplicates++;
                    dup.Add(tag);
                }
                
                Assert.That(tag > 0, Is.True, "Failed on {0}", tag);
                Assert.That(tag <= 16777216, Is.True, "Failed on {0}", tag);
            }
            var percent = (double)duplicates / iterations;
            Console.WriteLine("In {0} iterations, {1} duplicated tags. Percent {2}", iterations, duplicates, percent);

            // testing ability to adjust by one:

            var failes = 0;
            foreach (var tag in dup)
            {
                try
                {
                    dic.Add(tag + 1, Guid.Empty);
                }
                catch (Exception ex)
                {
                    failes++;
                }                
            }

            //percent = (double)failes / iterations;
            Console.WriteLine("In {0} duplicated tags, {1} can be easely fixed by +1.", dup.Count, dup.Count - failes);
        }
    }
}