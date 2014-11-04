using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Implementation.Persistence;
using Agile.Planning.Domain.Commands;
using Agile.Planning.Domain.Models;

namespace Agile.Web.Controllers
{
    public class StoriesController : ApiController
    {
        private readonly ICommandHandler<AddStoryCommand> _commandHandler;

        public StoriesController(ICommandHandler<AddStoryCommand> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task<IHttpActionResult> Get()
        {
            return await Post(new CreateStoryModel()
            {
                Title = "Some random title",
                Description = "Some random description"
            });
        }

        public async Task<IHttpActionResult> Post(CreateStoryModel model)
        {
            var command = new AddStoryCommand()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description
            };

            await _commandHandler.Handle(command);

            return Created(Url.Link("DefaultApi", new {controller = "Stories"}), new {command.Id});
        }
    }

    public class CreateStoryModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}