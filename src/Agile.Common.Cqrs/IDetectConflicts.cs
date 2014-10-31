using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs
{
    public interface IDetectConflicts
    {
        void Register<TUncommitted, TCommitted>(ConflictDelegate handler)
            where TUncommitted : class
            where TCommitted : class;

        bool ConflictsWith(IEnumerable<object> uncommittedEvents, IEnumerable<object> committedEvents);
    }

    public delegate bool ConflictDelegate(object uncommitted, object committed);
}
