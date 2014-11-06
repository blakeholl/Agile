using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Core;

namespace Agile.Planning.Domain.Models.Products
{
    public class Product : AggregateBase
    {
        // TODO: Register handlers
        private Product()
        {
            Register<ProductAdded>(Handle);
            Register<ProductStoryAdded>(Handle);
        }

        public Product(Guid id, string name) : this()
        {
            RaiseEvent(new ProductAdded(id, name));
        }

        private void Handle(ProductStoryAdded @event)
        {
            
        }

        private void Handle(ProductAdded @event)
        {
            Id = @event.Id;
            Name = @event.Name;
        }

        public void AddStory(Guid storyId, string title, string description)
        {
            RaiseEvent(new ProductStoryAdded(storyId, Id, title, description));
        }

        public string Name { get; private set; }
    }

    public class ProductAdded : IDomainEvent
    {
        public ProductAdded(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public readonly Guid Id;
        public readonly string Name;
    }
}
