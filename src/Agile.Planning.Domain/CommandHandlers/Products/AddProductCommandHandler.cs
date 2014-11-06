using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Persistence;
using Agile.Planning.Domain.Commands.Products;
using Agile.Planning.Domain.Models.Products;

namespace Agile.Planning.Domain.CommandHandlers.Products
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
    {
        private readonly IRepository _repository;

        public AddProductCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(AddProductCommand command)
        {
            var product = new Product(command.Id, command.Name);
            await _repository.Save(product, Guid.NewGuid());
        }
    }
}
