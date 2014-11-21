using System.Threading.Tasks;

namespace Agile.Common.EventPublishing
{
    public interface IEventSubscriber
    {

    }

    public interface IEventSubscriber<in TEvent> : IEventSubscriber
        where TEvent : class
    {
        Task Handle(TEvent @event);
    }
}
