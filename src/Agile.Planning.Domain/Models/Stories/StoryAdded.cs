using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Models.Stories
{
    public class StoryAdded : IDomainEvent
    {
        public StoryAdded(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        public readonly Guid Id;
        public readonly string Title;
        public readonly string Description;
    }
}