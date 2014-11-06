using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Commands.Stories
{
    public class DeleteStoryCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
