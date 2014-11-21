using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agile.Common.EventPublishing
{
    public class SubscriberFactoryPipeline<TEvent> : ISubscriberPipeline<TEvent> where TEvent : class
    {
        private readonly IList<Func<IEventSubscriber<TEvent>>> _subscriberFactories = new List<Func<IEventSubscriber<TEvent>>>();
        private readonly IContainer _container;

        public SubscriberFactoryPipeline(IContainer container)
        {
            _container = container;
        }

        public void AddSubscriberFactory(Func<IEventSubscriber<TEvent>> factory)
        {
            _subscriberFactories.Add(factory);
        }

        public void AddSubscriber<TSubscriber>() where TSubscriber : class, IEventSubscriber<TEvent>
        {
            _subscriberFactories.Add(() => _container.Get<TSubscriber>());
        }

        public async Task Send(object @event)
        {
            var typedEvent = @event as TEvent;

            foreach (var subscriberFactory in _subscriberFactories)
            {
                var subscriber = subscriberFactory();
                await subscriber.Handle(typedEvent);
            }
        }
    }
}
