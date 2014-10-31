using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs.Persistence
{
    public interface IConflictWith
    {
        bool ConflictsWith(object uncommitted, object committed);
    }
}
