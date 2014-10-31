using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs.Persistence
{
    public interface IRepository
    {
        Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;
        Task<TAggregate> GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
        Task Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
    }
}
