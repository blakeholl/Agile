using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Persistence;
using Agile.Planning.Domain.Commands;
using Agile.Planning.Domain.Models;
using Agile.Planning.Domain.Models.Stories;

namespace Agile.Planning.Domain.CommandHandlers
{
    public class DeleteStoryCommandHandler : ICommandHandler<DeleteStoryCommand>
    {
        private readonly IRepository _repository;

        public DeleteStoryCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteStoryCommand command)
        {
            var story = await _repository.GetById<Story>(command.Id);
            story.Delete();
            await _repository.Save(story, Guid.NewGuid());
        }
    }
}
