using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agile.Common.EventPublishing
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IDictionary<Type, ISubscriberPipeline> _pipelineRegistry = new Dictionary<Type, ISubscriberPipeline>();

        public void AddPipeline<TEvent>(ISubscriberPipeline<TEvent> pipeline) where TEvent : class
        {
            _pipelineRegistry.Add(typeof(TEvent), pipeline);
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : class
        {
            ISubscriberPipeline subscriberPipeline;
            if (!_pipelineRegistry.TryGetValue(typeof(TEvent), out subscriberPipeline))
            {
                throw new Exception("Pipeline not registered for type: " + typeof(TEvent).AssemblyQualifiedName);
            }

            await subscriberPipeline.Send(@event);
        }
    }
}
