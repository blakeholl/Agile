using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agile.Common.EventPublishing
{
    public class SingletonSubscriberPipeline<TEvent> : ISubscriberPipeline<TEvent> where TEvent : class
    {
        private readonly IList<IEventSubscriber<TEvent>> _handlers;

        public SingletonSubscriberPipeline(IList<IEventSubscriber<TEvent>> handlers)
        {
            _handlers = handlers;
        }

        public async Task Send(object @event)
        {
            var typedEvent = @event as TEvent;

            foreach (var handler in _handlers)
            {
                await handler.Handle(typedEvent);
            }
        }
    }
}
