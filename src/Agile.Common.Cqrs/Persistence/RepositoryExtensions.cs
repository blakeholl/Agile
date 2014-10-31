using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs.Persistence
{
    public static class RepositoryExtensions
    {
        public static void Save(this IRepository repository, IAggregate aggregate, Guid commitId)
        {
            repository.Save(aggregate, commitId, a => { });
        }
    }
}
