using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs.Core;

namespace Agile.Planning.Domain.Models
{
    public class Story : AggregateBase
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        private Story()
        {
            Register<StoryAdded>(Handle);
            Register<StoryTitleChanged>(Handle);
        }

        public Story(Guid id, string title, string description)
            : this()
        {
            RaiseEvent(new StoryAdded(id, title, description));
        }

        private void Handle(StoryAdded @event)
        {
            Id = @event.Id;
            Title = @event.Title;
            Description = @event.Description;
        }

        private void Handle(StoryTitleChanged @event)
        {
            Title = @event.Title;
        }

        public void ChangeTitle(string title)
        {
            RaiseEvent(new StoryTitleChanged(title));
        }
    }

    public class StoryTitleChanged
    {
        public StoryTitleChanged(string title)
        {
            Title = title;
        }

        public readonly string Title;
    }

    public class StoryAdded
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
