using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Agile.Common.Cqrs;
using Agile.Planning.DataTransfer.Story;
using Agile.Planning.Domain.Commands;

namespace Agile.Web.Controllers
{
    [RoutePrefix("api/stories")]
    public class StoriesController : ApiController
    {
        private readonly ICommandHandler<AddStoryCommand> _addStoryCommandHandler;
        private readonly ICommandHandler<ChangeStoryTitleCommand> _changeStoryTitleCommandHandler;
        private readonly ICommandHandler<DeleteStoryCommand> _deleteStoryCommandHandler;

        public StoriesController(ICommandHandler<AddStoryCommand> addStoryCommandHandler, 
            ICommandHandler<ChangeStoryTitleCommand> changeStoryTitleCommandHandler, 
            ICommandHandler<DeleteStoryCommand> deleteStoryCommandHandler)
        {
            _addStoryCommandHandler = addStoryCommandHandler;
            _changeStoryTitleCommandHandler = changeStoryTitleCommandHandler;
            _deleteStoryCommandHandler = deleteStoryCommandHandler;
        }

        [Route, HttpPost]
        public async Task<IHttpActionResult> CreateStory(CreateStoryModel model)
        {
            await _addStoryCommandHandler.Handle(new AddStoryCommand()
            {
                Id = model.Id,
                Description = model.Description, 
                Title = model.Title
            });

            return Created(Url.Link("DefaultApi", new {controller = "Stories"}), new {model.Id});
        }

        [Route("{id:guid}/rename"), HttpPost]
        public async Task<IHttpActionResult> RenameStory(Guid id, RenameStoryModel model)
        {
            await _changeStoryTitleCommandHandler.Handle(new ChangeStoryTitleCommand()
            {
                Id = id,
                Title = model.Title
            });

            return Ok();
        }

        [Route("{id:guid}"), HttpDelete]
        public async Task<IHttpActionResult> DeleteStory(Guid id)
        {
            await _deleteStoryCommandHandler.Handle(new DeleteStoryCommand()
            {
                Id = id
            });

            return Ok();
        }
    }
}