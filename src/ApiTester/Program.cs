using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace ApiTester
{
    class Program
    {
        private const int TestSize = 5000;
        private const int TesterCount = 20;
        private static readonly BlockingCollection<int> Tests = new BlockingCollection<int>();

        static void Main(string[] args)
        {
            RunTestsAsync();

            Console.WriteLine("donezo");
            Console.ReadLine();
        }

        public static void RunTestsAsync()
        {
            Task.Factory.StartNew(() =>
            {
                for (var testIndex = 0; testIndex < TestSize; testIndex++)
                {
                    Tests.Add(testIndex);
                }

                Tests.CompleteAdding();
            });

            for (var testerIndex = 0; testerIndex < TesterCount; testerIndex++)
            {
                Task.Factory.StartNew(Consume);
            }
        }

        private async static void Consume()
        {
            var tester = new Tester(new HttpClient() { BaseAddress = new Uri("http://localhost:7656") });

            foreach (var i in Tests.GetConsumingEnumerable())
            {
                Console.WriteLine("Thread {0} Consuming: {1}", Thread.CurrentThread.ManagedThreadId, i);
                await tester.RunTest();
                Console.WriteLine("done");
            }
        }
    }
}
