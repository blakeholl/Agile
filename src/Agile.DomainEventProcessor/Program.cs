using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using Agile.Common.Cqrs;
using Agile.Common.EventPublishing;
using Agile.Planning.Domain.Models.Products;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ninject;

namespace Agile.DomainEventProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = ConfigureKernel();
            var eventStorePublisher = kernel.Get<EventStorePublisher>();
            eventStorePublisher.Start();
            Console.ReadLine();
        }

        static IKernel ConfigureKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IEventStoreConnection>()
                .ToMethod(x =>
                {
                    var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
                    connection.ConnectAsync().Wait();
                    return connection;
                })
                .InSingletonScope();

            return kernel;
        }
    }
}