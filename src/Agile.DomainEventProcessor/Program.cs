using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Ninject;

namespace Agile.DomainEventProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = ConfigureKernel();
            var connection = kernel.Get<IEventStoreConnection>();
            Task.Factory.StartNew(() => ConfigureSub(connection));
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

        static async Task ConfigureSub(IEventStoreConnection eventStoreConnection)
        {
            var sub =
                await
                    eventStoreConnection.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped,
                        new UserCredentials("admin", "changeit"));
        }

        private static void SubscriptionDropped(EventStoreSubscription eventStoreSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {
            throw new NotImplementedException();
        }

        private static void EventAppeared(EventStoreSubscription eventStoreSubscription, ResolvedEvent resolvedEvent)
        {
            Console.WriteLine(resolvedEvent.Event.Data);
        }
    }
}
