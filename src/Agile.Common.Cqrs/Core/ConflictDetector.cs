using System;
using System.Collections.Generic;
using System.Linq;

namespace Agile.Common.Cqrs.Core
{
    public class ConflictDetector : IDetectConflicts
    {
        private readonly IDictionary<Type, IDictionary<Type, ConflictDelegate>> _actions =
            new Dictionary<Type, IDictionary<Type, ConflictDelegate>>();

        public void Register<TUncommitted, TCommitted>(ConflictDelegate handler)
            where TUncommitted : class
            where TCommitted : class
        {
            IDictionary<Type, ConflictDelegate> inner;
            if (!_actions.TryGetValue(typeof(TUncommitted), out inner))
                _actions[typeof(TUncommitted)] = inner = new Dictionary<Type, ConflictDelegate>();

            inner[typeof(TCommitted)] = (uncommitted, committed) =>
                handler(uncommitted as TUncommitted, committed as TCommitted);
        }

        public bool ConflictsWith(IEnumerable<object> uncommittedEvents, IEnumerable<object> committedEvents)
        {
            return (from object uncommitted in uncommittedEvents
                    from object committed in committedEvents
                    where Conflicts(uncommitted, committed)
                    select uncommittedEvents).Any();
        }
        private bool Conflicts(object uncommitted, object committed)
        {
            IDictionary<Type, ConflictDelegate> registration;
            if (!_actions.TryGetValue(uncommitted.GetType(), out registration))
                return uncommitted.GetType() == committed.GetType(); // no reg, only conflict if the events are the same time

            ConflictDelegate callback;
            if (!registration.TryGetValue(committed.GetType(), out callback))
                return true;

            return callback(uncommitted, committed);
        }
    }
}
