using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs.Core;
using Agile.Planning.DataTransfer.Story;
using Agile.Planning.Domain.Events;

namespace Agile.Planning.Domain.Models
{
    public class Story : AggregateBase
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public StoryStatus Status { get; private set; }

        private Story()
        {
            Register<StoryAdded>(Handle);
            Register<StoryTitleChanged>(Handle);
            Register<StoryDeleted>(Handle);
        }

        public Story(Guid id, string title, string description)
            : this()
        {
            RaiseEvent(new StoryAdded(id, title, description));
        }

        private void Handle(StoryAdded @event)
        {
            Id = @event.Id;
            Status = StoryStatus.Created;
            Title = @event.Title;
            Description = @event.Description;
        }

        private void Handle(StoryTitleChanged @event)
        {
            Title = @event.Title;
        }

        private void Handle(StoryDeleted @event)
        {
            Status = StoryStatus.Deleted;
        }

        public void ChangeTitle(string title)
        {
            RaiseEvent(new StoryTitleChanged(Id, title));
        }

        public void Delete()
        {
            if (Status != StoryStatus.Created)
            {
                throw new InvalidOperationException("Can only perform this operation when in Created status");
            }

            RaiseEvent(new StoryDeleted(Id));
        }
    }
}
