using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using ZMQ;

namespace Paralect.Machine.Tests.Areas.Zeromq
{
    [TestFixture]
    public class SimpleTest
    {

        private const string Address = "inproc://test";
        private const uint MessageSize = 10;
        private const int RoundtripCount = 100;

        private static Context ctx;

        [Test]
        public void Should()
        {
            using (ctx = new Context(1))
            {
                var clientThread = new Thread(StartClient);
                clientThread.Start();

                var serverThread = new Thread(StartServer);
                serverThread.Start();

                clientThread.Join();
                serverThread.Join();

                Console.WriteLine("Done with life");
            }
        }

        private void StartServer()
        {
            try
            {
                using (var skt = ctx.Socket(SocketType.REP))
                {
                    skt.Bind(Address);

                    Console.WriteLine("Server has bound");

                    //  Bounce the messages.
                    for (var i = 0; i < RoundtripCount; i++)
                    {
                        var msg = skt.Recv();
                        Debug.Assert(msg.Length == MessageSize);
                        skt.Send(msg);
                    }
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Done with server");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void StartClient()
        {
            Thread.Sleep(2000);

            try
            {
                //  Initialise 0MQ
                using (var skt = ctx.Socket(SocketType.REQ))
                {
                    skt.Connect(Address);

                    Console.WriteLine("Client has bound");

                    //  Create a message to send.
                    var msg = new byte[MessageSize];

                    //  Start measuring the time.
                    var watch = new Stopwatch();
                    watch.Start();

                    //  Start sending messages.
                    for (var i = 0; i < RoundtripCount; i++)
                    {
                        skt.Send(msg);
                        msg = skt.Recv();
                        Debug.Assert(msg.Length == MessageSize);

                        Console.Write(".");
                    }

                    //  Stop measuring the time.
                    watch.Stop();
                    var elapsedTime = watch.ElapsedTicks;

                    //  Print out the test parameters.
                    Console.WriteLine("message size: " + MessageSize + " [B]");
                    Console.WriteLine("roundtrip count: " + RoundtripCount);

                    //  Compute and print out the latency.
                    var latency = (double)(elapsedTime) / RoundtripCount / 2 *
                                  1000000 / Stopwatch.Frequency;
                    Console.WriteLine("Your average latency is {0} [us]",
                                      latency.ToString("f2"));
                }

                Console.WriteLine("Done with client");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}