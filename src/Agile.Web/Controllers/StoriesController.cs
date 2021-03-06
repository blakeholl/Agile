﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Agile.Common.Cqrs;
using Agile.Planning.DataTransfer.Story;
using Agile.Planning.Domain.Commands;
using Agile.Planning.Domain.Commands.Products;
using Agile.Planning.Domain.Commands.Stories;

namespace Agile.Web.Controllers
{
    [RoutePrefix("api/stories")]
    public class StoriesController : ApiController
    {
        
        private readonly ICommandHandler<ChangeStoryTitleCommand> _changeStoryTitleCommandHandler;
        private readonly ICommandHandler<DeleteStoryCommand> _deleteStoryCommandHandler;

        public StoriesController(ICommandHandler<AddStoryCommand> addStoryCommandHandler, 
            ICommandHandler<ChangeStoryTitleCommand> changeStoryTitleCommandHandler, 
            ICommandHandler<DeleteStoryCommand> deleteStoryCommandHandler)
        {
            _changeStoryTitleCommandHandler = changeStoryTitleCommandHandler;
            _deleteStoryCommandHandler = deleteStoryCommandHandler;
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