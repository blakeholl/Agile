﻿using System;
using System.Threading.Tasks;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Persistence;
using Agile.Planning.Domain.Commands;
using Agile.Planning.Domain.Commands.Stories;
using Agile.Planning.Domain.Models.Stories;

namespace Agile.Planning.Domain.CommandHandlers.Stories
{
    public class ChangeStoryTitleCommandHandler : ICommandHandler<ChangeStoryTitleCommand>
    {
        private readonly IRepository _repository;

        public ChangeStoryTitleCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(ChangeStoryTitleCommand command)
        {
            var story = await _repository.GetById<Story>(command.Id);
            story.ChangeTitle(command.Title);
            await _repository.Save(story, Guid.NewGuid());
        }
    }
}
