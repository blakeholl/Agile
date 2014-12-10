using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Agile.Common.EventPublishing;
using Agile.Planning.Domain.Models.Products;
using Agile.Planning.EventHandlers;
using EventStore.ClientAPI;
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

            kernel.Bind<IEventPublisher>()
                .ToMethod(x =>
                {
                    var eventPublisher = new EventPublisher();
                    var container = new FakeContainer(Activator.CreateInstance);

                    var eventAPipeline = new SubscriberFactoryPipeline<ProductAdded>(container);
                    eventAPipeline.AddSubscriber<ProductAddedSubscriber>();
                    eventPublisher.AddPipeline(eventAPipeline);

                    //var eventBPipeline = new SubscriberFactoryPipeline<EventB>(container);
                    //eventBPipeline.AddSubscriberFactory(() => new SomeEventBSubscriber());
                    //eventBPipeline.AddSubscriberFactory(() => new AnotherEventBSubscriber());
                    //eventPublisher.AddPipeline(eventBPipeline);

                    //var eventCPipeline = new SubscriberFactoryPipeline<EventC>(container);
                    //eventCPipeline.AddSubscriberFactory(() => new SomeEventCSubscriber());
                    //eventPublisher.AddPipeline(eventCPipeline);

                    return eventPublisher;

                }).InSingletonScope();

            return kernel;
        }
    }
}