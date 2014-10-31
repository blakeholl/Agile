using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs.Persistence
{
    public interface IConstructAggregates
    {
        IAggregate Build(Type type, Guid id, IMemento snapshot);
    }
}
