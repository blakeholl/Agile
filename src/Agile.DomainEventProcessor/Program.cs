using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string EventClrTypeHeader = "EventClrTypeName";
        private static readonly Dictionary<Type, Action<object>> Handlers = new Dictionary<Type, Action<object>>();

        static void Main(string[] args)
        {
            Action<object> productAddedHandler = o => Console.WriteLine(o.GetType());
            Handlers.Add(typeof (ProductAdded), productAddedHandler);

            var kernel = ConfigureKernel();
            var connection = kernel.Get<IEventStoreConnection>();
            ConfigureSub(connection);
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

        static void ConfigureSub(IEventStoreConnection eventStoreConnection)
        {
            eventStoreConnection.SubscribeToAllFrom(Position.Start, false, EventAppeared, LiveProcessingStarted,
                SubscriptionDropped, new UserCredentials("admin", "changeit"));
        }

        private static void SubscriptionDropped(EventStoreCatchUpSubscription eventStoreCatchUpSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {
            
        }

        private static void LiveProcessingStarted(EventStoreCatchUpSubscription eventStoreCatchUpSubscription)
        {
            
        }

        private static void EventAppeared(EventStoreCatchUpSubscription eventStoreCatchUpSubscription, ResolvedEvent resolvedEvent)
        {
            if (IsSystemEvent(resolvedEvent))
            {
                return;
            }

            var wrapper = DeserializeEvent(resolvedEvent);
            HandleEvent(wrapper);
        }

        private static void HandleEvent(EventWrapper eventWrapper)
        {
            Action<object> handler;

            if (!Handlers.TryGetValue(eventWrapper.Event.GetType(), out handler))
            {
                return;
            }

            handler(eventWrapper.Event);
        }

        private static EventWrapper DeserializeEvent(ResolvedEvent resolvedEvent)
        {
            var metadata = JObject.Parse(Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Metadata));
            var eventClrTypeName = metadata.Property(EventClrTypeHeader).Value.ToString();
            var aggregateClrTypeName = metadata.Property(AggregateClrTypeHeader).Value.ToString();
            var eventType = Type.GetType(eventClrTypeName);

            return new EventWrapper()
            {
                AggregateType = aggregateClrTypeName,
                AggregateId = Guid.Parse(resolvedEvent.OriginalEvent.EventStreamId.Split('-').Last()),
                Event = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Data), eventType)
            };

        }

        private static bool IsSystemEvent(ResolvedEvent resolvedEvent)
        {
            return resolvedEvent.Event.EventType.StartsWith("$");
        }
    }
}