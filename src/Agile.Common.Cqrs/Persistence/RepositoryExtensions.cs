using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs.Persistence
{
    public static class RepositoryExtensions
    {
        public async static Task Save(this IRepository repository, IAggregate aggregate, Guid commitId)
        {
            await repository.Save(aggregate, commitId, a => { });
        }
    }
}
