using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Commands.Products
{
    public class AddStoryCommand : ICommand
    {
        public Guid ProductId { get; set; }
        public Guid StoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
