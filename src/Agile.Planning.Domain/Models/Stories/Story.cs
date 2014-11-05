using System;
using Agile.Common.Cqrs.Core;
using Agile.Planning.DataTransfer.Story;

namespace Agile.Planning.Domain.Models.Stories
{
    public class Story : AggregateBase
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public StoryStatus Status { get; private set; }
        public Guid? SprintId { get; private set; }

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

        public bool IsCommittedToSprint()
        {
            return SprintId.HasValue;
        }
    }
}
