using System;
using Agile.Common.EventPublishing;

namespace Agile.DomainEventProcessor
{
    public class FakeContainer : IContainer
    {
        private readonly Func<Type, object> _factoryFunc;

        public FakeContainer(Func<Type, object> factoryFunc)
        {
            _factoryFunc = factoryFunc;
        }

        public T Get<T>() where T : class
        {
            return _factoryFunc(typeof(T)) as T;
        }
    }
}
