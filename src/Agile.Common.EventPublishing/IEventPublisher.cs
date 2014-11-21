using System.Threading.Tasks;

namespace Agile.Common.EventPublishing
{
    public interface IEventPublisher
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : class;
    }
}
