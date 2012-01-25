using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;

namespace Paralect.Machine.Tests.Areas.Performance.Fixtures
{
    public class Program
    {
        public List<int> z = null; //new List<int>();

        public void Foo()
        {
            z = new List<int>();
            z.Clear();
            z.Add(1);
            z.Add(2);
        }
    }

    [TestFixture]
    public class DynamicInvocationPerformanceTest
    {
        [Ignore]
        public void ddddd()
        {
            const int maxAttempt = 1000000;
            var stopwatch = new Stopwatch();

            #region Normal Invocation
            var prog = new Program();
            stopwatch.Start();
            for (int i = 0; i < maxAttempt; i++)
            {
                prog.Foo();
            }
            stopwatch.Stop();
            Console.WriteLine("Normal Invocation took {0} milliseconds",
                              stopwatch.ElapsedMilliseconds);
            #endregion

            #region Using Reflection
            Type t = prog.GetType();
            stopwatch.Restart();
            for (int i = 0; i < maxAttempt; i++)
            {
                t.InvokeMember("Foo", BindingFlags.InvokeMethod, null, prog, new object[] { });
            }
            stopwatch.Stop();
            Console.WriteLine("Using reflection took {0} milliseconds",
                              stopwatch.ElapsedMilliseconds);
            #endregion

            #region Dynamic Invocation
            dynamic dynamicProg = prog;

            stopwatch.Restart();
            for (int i = 0; i < maxAttempt; i++)
            {
                dynamicProg.Foo();
            }
            stopwatch.Stop();
            Console.WriteLine("Dynamic Invocation took {0} milliseconds",
                              stopwatch.ElapsedMilliseconds);
            #endregion

            // TODO: here is another aproach to speedup reflection:
            // http://msmvps.com/blogs/jon_skeet/archive/2008/08/09/making-reflection-fly-and-exploring-delegates.aspx
            // TODO: test it!

//            MethodInfo method = typeof(Program).GetMethod("Foo");
//            Action converted = (Action)Delegate.CreateDelegate(typeof(Action), method);



            
        }

    }
}