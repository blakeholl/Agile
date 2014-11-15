using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        static void Main(string[] args)
        {
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

            Console.WriteLine(resolvedEvent.OriginalEvent.EventStreamId);
            Console.WriteLine(Guid.Parse(resolvedEvent.OriginalStreamId.Split('-').Last()).ToString("B"));
            Console.WriteLine(resolvedEvent.OriginalEventNumber);
            var @event = DeserializeEvent(resolvedEvent.Event.Metadata, resolvedEvent.Event.Data);
            Console.WriteLine(@event.GetType().FullName);
        }

        private static dynamic DeserializeEvent(byte[] metadata, byte[] data)
        {
            var meta = JObject.Parse(Encoding.UTF8.GetString(metadata));

            var aggregateClrTypeName = meta.Property(AggregateClrTypeHeader).Value;
            var eventClrTypeName = meta.Property(EventClrTypeHeader).Value;

            Console.WriteLine("{0} {1}", aggregateClrTypeName, eventClrTypeName);
            
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
        }

        private static bool IsSystemEvent(ResolvedEvent resolvedEvent)
        {
            return resolvedEvent.Event.EventType.StartsWith("$");
        }
    }
}
