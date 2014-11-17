using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Models.Products
{
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