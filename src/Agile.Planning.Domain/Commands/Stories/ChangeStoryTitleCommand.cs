using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Commands.Stories
{
    public class ChangeStoryTitleCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
