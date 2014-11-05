using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Models.Stories
{
    public class StoryDeleted : IDomainEvent
    {
        public StoryDeleted(Guid id)
        {
            Id = id;
        }

        public readonly Guid Id;
    }
}