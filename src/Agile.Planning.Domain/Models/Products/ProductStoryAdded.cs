using System;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Models.Products
{
    public class ProductStoryAdded : IDomainEvent
    {
        public ProductStoryAdded(Guid id, Guid productId, string title, string description)
        {
            Id = id;
            ProductId = productId;
            Title = title;
            Description = description;
        }

        public readonly Guid Id;
        public readonly Guid ProductId;
        public readonly string Title;
        public readonly string Description;
    }
}