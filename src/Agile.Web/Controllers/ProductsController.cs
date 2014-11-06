using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Agile.Common.Cqrs;
using Agile.Planning.DataTransfer.Products;
using Agile.Planning.Domain.Commands.Products;

namespace Agile.Web.Controllers
{
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private readonly ICommandHandler<AddStoryCommand> _addStoryCommandHandler;
        private readonly ICommandHandler<AddProductCommand> _addProductCommandHandler;

        public ProductsController(ICommandHandler<AddStoryCommand> addStoryCommandHandler, ICommandHandler<AddProductCommand> addProductCommandHandler)
        {
            _addStoryCommandHandler = addStoryCommandHandler;
            _addProductCommandHandler = addProductCommandHandler;
        }

        [Route, HttpPost]
        public async Task<IHttpActionResult> CreateProduct(CreateProductModel model)
        {
            await _addProductCommandHandler.Handle(new AddProductCommand()
            {
                Id = model.Id,
                Name = model.Name
            });

            return Created(Url.Link("DefaultApi", new { controller = "Products" }), new { model.Id });
        }

        [Route("{id:guid}/Stories"), HttpPost]
        public async Task<IHttpActionResult> CreateStory(Guid id, CreateStoryModel model)
        {
            await _addStoryCommandHandler.Handle(new AddStoryCommand()
            {
                ProductId = id,
                StoryId = model.Id,
                Description = model.Description,
                Title = model.Title
            });

            return Created(Url.Link("DefaultApi", new { controller = "Stories" }), new { model.Id });
        }
    }
}
