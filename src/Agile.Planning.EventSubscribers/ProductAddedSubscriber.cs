using System;
using System.Threading.Tasks;
using Agile.Common.EventPublishing;
using Agile.Planning.Domain.Models.Products;

namespace Agile.Planning.EventSubscribers
{
    public class ProductAddedSubscriber : IEventSubscriber<ProductAdded>
    {
        public async Task Handle(ProductAdded @event)
        {
            Console.WriteLine("Product Added: " + @event.Id);
        }
    }
}
