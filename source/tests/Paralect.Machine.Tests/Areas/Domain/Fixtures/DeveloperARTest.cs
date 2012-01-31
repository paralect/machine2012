using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Paralect.Machine.Messages;
using Paralect.Machine.Processes;
using Paralect.Machine.Tests.Areas.Domain.Aggregates;

namespace Paralect.Machine.Tests.Areas.Domain.Fixtures
{
    [TestFixture]
    public class DeveloperARTest
    {
        public void test_it()
        {
            Type idtype = typeof (DeveloperId);
            Type type = typeof (DeveloperState);

            object z = Activator.CreateInstance(type);
            var state = (DeveloperState) z;
            var state_d = (dynamic) z;

            var e1 = new DeveloperCreated {Name = "Vasya"};
            var e2 = new DeveloperCreated {Name = "Dima"};

            Type eventTypeTemplate = typeof (IEvent<>);
            Type eventType = eventTypeTemplate.MakeGenericType(idtype);

            Type listTypeTemplate = typeof (List<>);
            Type listType = listTypeTemplate.MakeGenericType(eventType);

            Type enumTypeTemplate = typeof (IEnumerable<>);
            Type enumType = enumTypeTemplate.MakeGenericType(eventType);

            object list = Activator.CreateInstance(listType);
            var d_list = (dynamic) list;
            d_list.Add(e1);
            d_list.Add(e2);

/*             var watch = Stopwatch.StartNew();
             for (int i = 0; i < 1000000; i++)
             {
                 state_d.Apply(d_list);

             }
             watch.Stop();
             Console.WriteLine("2) {0}", watch.ElapsedMilliseconds);

             watch = Stopwatch.StartNew();
             for (int i = 0; i < 1000000; i++)
             {
                 state.Apply((IEnumerable<IEvent<DeveloperId>>) list);
             }
             watch.Stop();
             Console.WriteLine("1) {0}", watch.ElapsedMilliseconds);

             var method = type.GetMethod("Apply", new Type[] { enumType });

             watch = Stopwatch.StartNew();
             for (int i = 0; i < 1000000; i++)
             {
                 method.Invoke(state, new object[] { (IEnumerable<IEvent<DeveloperId>>)list});
//                 state.Apply();
             }
             watch.Stop();
             Console.WriteLine("3) {0}", watch.ElapsedMilliseconds);
             */


            //var zzz = (IEvent) e1;

            //var list = new List<IEvent>() { e1, e2 };


            //state.Apply2(e1);

            //dynamic.Apply(list);
        }

        [Test]
        public void test2()
        {
            var e1 = new DeveloperCreated { Name = "Vasya" };
            var e2 = new DeveloperCreated { Name = "Dima" };

            var state = new DeveloperState();
            state.Apply(e1, e2);
            state.Apply(e1);
            state.Apply(e1, e2);
            state.Apply(e1);


            var factory = new ProcessFactory();
            var agr = factory.Create<DeveloperAR>();

            var result = agr.Execute(new ChangeDeveloperName() { NewName = "Cohen" }, null, state);

            Assert.That(((DeveloperNameChanged)result.First()).NewName, Is.EqualTo("Cohen"));


        }
    }
}