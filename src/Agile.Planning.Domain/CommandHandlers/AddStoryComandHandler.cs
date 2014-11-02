using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Persistence;
using Agile.Planning.Domain.Commands;
using Agile.Planning.Domain.Models;

namespace Agile.Planning.Domain.CommandHandlers
{
    public class AddStoryComandHandler : ICommandHandler<AddStoryCommand>
    {
        private readonly IRepository _repository;

        public AddStoryComandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(AddStoryCommand command)
        {
            var story = new Story(command.Id, command.Title, command.Description);

            await _repository.Save(story, Guid.NewGuid());
        }
    }
}
