using System;
using Agile.Common.Cqrs.Core;

namespace Agile.Planning.Domain.Models.Sprints
{
    public class Sprint : AggregateBase
    {
        // TODO: Register handlers
        private Sprint()
        {
            
        }

        public Sprint(Guid id, string name) : this()
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }
        
    }
}
