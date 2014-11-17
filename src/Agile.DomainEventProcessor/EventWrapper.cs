using System;

namespace Agile.DomainEventProcessor
{
    public class EventWrapper
    {
        public string AggregateType { get; set; }
        public Guid AggregateId { get; set; }
        public object Event { get; set; }
    }
}