﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Paralect.Machine.Nodes;
using ZMQ;
using Exception = System.Exception;

namespace Paralect.Machine.Tests.Areas.Engine
{
    [TestFixture]
    public class HostTest
    {
        [Ignore("Test takes time, run this test manually")]
        public void simple_test()
        {
            var context = new Context(2);
            var engine = new Host(
                //new ServerEngineProcess(context, "inproc://test"),
                //new ClientEngineProcess(context, "inproc://test", 10000),
                new ServerNode(context, "tcp://*:5567"),
                new ClientNode(context, "tcp://localhost:5567", 10000)
            );

            using (var token = new CancellationTokenSource())
            {
                var task1 = engine.Start(token.Token, 5000);

                if (task1.Wait(5000))
                    Console.WriteLine("Done without forced cancelation"); // This line shouldn't be reached
                else
                    Console.WriteLine("\r\nRequesting to cancel...");

                token.Cancel();
            }
        }
    }

    public class ServerNode : INode
    {
        private readonly Context _context;
        private readonly String _address;

        public ServerNode(Context context, String address)
        {
            _context = context;
            _address = address;
        }

        public void Dispose() { }
        public void Init() { }
        public void Run(CancellationToken token)
        {
            try
            {
                using (var skt = _context.Socket(SocketType.REP))
                {
                    Thread.Sleep(200); // simulate late bind
                    skt.Bind(_address);

                    Console.WriteLine();
                    Console.WriteLine("Server has bound");
                        

                    while (!token.IsCancellationRequested)
                    {
                        var msg = skt.Recv(200);
                            

                        if (msg == null)
                            continue;

                        skt.Send(msg);
                    }

                    Console.WriteLine("Done with server");
                        
                }
            }
            catch (ObjectDisposedException)
            {
                // suppress
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class ClientNode : INode
    {
        private readonly Context _context;
        private int _roundtrips;
        private readonly String _address;

        public ClientNode(Context context, String address, int roundtrips)
        {
            _context = context;
            _roundtrips = roundtrips;
            _address = address;
        }

        public void Dispose() { }
        public void Init() { }
        public void Run(CancellationToken token)
        {
            try
            {

                //  Initialise 0MQ
                using (var skt = _context.Socket(SocketType.REQ))
                {


                    // Try to connect 
                    Console.Write("Connecting: ");
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            skt.Connect(_address);
                            break;
                        }
                        catch(ZMQ.Exception ex)
                        {
                            // Connection refused
                            if (ex.Errno == 107)
                            {
                                Console.Write("*");
                                Func<Boolean> f = () => token.IsCancellationRequested;
                                SpinWait.SpinUntil(f, 100);
//                                    Thread.Sleep(300);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }

                        

                    Console.WriteLine("Client has connected!");
                    Console.Write("Replies (messages): ");

                    //  Create a message to send.
                    var msg = new byte[300];

                    //  Start measuring the time.
                    var watch = new Stopwatch();
                    watch.Start();

                    int i = 0;
                    while (!token.IsCancellationRequested)
                    {
                        i++;

                        if (i >= _roundtrips)
                            break;

                        skt.Send(msg);

                        while (!token.IsCancellationRequested)
                        {
                            msg = skt.Recv(300);

                            if (msg != null)
                                break;
                        }

                        // Reply received
                        Console.Write(".");
                    }

                    //  Stop measuring the time.
                    watch.Stop();
                    var elapsedTime = watch.ElapsedTicks;

                    Console.WriteLine();
                    //  Print out the test parameters.
                    Console.WriteLine("message size: " + 30 + " [B]");
                    Console.WriteLine("roundtrip count: " + _roundtrips);
                    Console.WriteLine("Time (ms): " + watch.ElapsedMilliseconds);

                    //  Compute and print out the latency.
                    var latency = (double)(elapsedTime) / _roundtrips / 2 *
                                    1000000 / Stopwatch.Frequency;
                    Console.WriteLine("Your average latency is {0} [us]",
                                        latency.ToString("f2"));
                }

                Console.WriteLine("Done with client");
            }
            catch (ObjectDisposedException)
            {
                // suppress
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}