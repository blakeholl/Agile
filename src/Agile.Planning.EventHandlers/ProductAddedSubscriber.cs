using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.EventPublishing;
using Agile.Planning.Domain.Models.Products;

namespace Agile.Planning.EventHandlers
{
    public class ProductAddedSubscriber : IEventSubscriber<ProductAdded>
    {
        public async Task Handle(ProductAdded @event)
        {
            Console.WriteLine("Product Added!");
        }
    }
}
