using System.Threading.Tasks;

namespace Agile.Common.EventPublishing
{
    public interface ISubscriberPipeline
    {
        Task Send(object @event);
    }

    public interface ISubscriberPipeline<T> : ISubscriberPipeline { }
}
