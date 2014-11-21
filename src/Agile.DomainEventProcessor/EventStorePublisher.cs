using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.EventPublishing;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Agile.DomainEventProcessor
{
    public class EventStorePublisher
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IEventPublisher _eventPublisher;
        private const string EventClrTypeHeader = "EventClrTypeName";

        public EventStorePublisher(IEventStoreConnection eventStoreConnection, IEventPublisher eventPublisher)
        {
            _eventStoreConnection = eventStoreConnection;
            _eventPublisher = eventPublisher;
        }

        public void Start()
        {
            _eventStoreConnection.SubscribeToAllFrom(Position.Start, false, EventAppeared, LiveProcessingStarted,
                SubscriptionDropped, new UserCredentials("admin", "changeit"));
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription eventStoreCatchUpSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {

        }

        private void LiveProcessingStarted(EventStoreCatchUpSubscription eventStoreCatchUpSubscription)
        {

        }

        private void EventAppeared(EventStoreCatchUpSubscription eventStoreCatchUpSubscription,
            ResolvedEvent resolvedEvent)
        {
            if (IsSystemEvent(resolvedEvent))
            {
                return;
            }

            var @event = DeserializeEvent(resolvedEvent);
            _eventPublisher.Publish((dynamic) @event).GetAwaiter().GetResult();
        }

        private static object DeserializeEvent(ResolvedEvent resolvedEvent)
        {
            var metadata = JObject.Parse(Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Metadata));
            var eventClrTypeName = metadata.Property(EventClrTypeHeader).Value.ToString();
            var eventType = Type.GetType(eventClrTypeName);
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Data), eventType);
        }

        private static bool IsSystemEvent(ResolvedEvent resolvedEvent)
        {
            return resolvedEvent.Event.EventType.StartsWith("$");
        }
    }
}
