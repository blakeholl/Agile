using System;
using System.Threading.Tasks;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Persistence;
using Agile.Planning.Domain.Commands.Products;
using Agile.Planning.Domain.Models.Products;

namespace Agile.Planning.Domain.CommandHandlers.Stories
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
            var product = await _repository.GetById<Product>(command.ProductId);
            product.AddStory(command.StoryId, command.Title, command.Description);
            await _repository.Save(product, Guid.NewGuid());
        }
    }
}
