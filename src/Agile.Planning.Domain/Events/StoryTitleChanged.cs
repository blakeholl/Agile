using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Events
{
    public class StoryTitleChanged : IDomainEvent
    {
        public StoryTitleChanged(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public readonly Guid Id;
        public readonly string Title;
    }
}