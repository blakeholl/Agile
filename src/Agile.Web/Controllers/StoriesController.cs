using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Agile.Common.Cqrs.Implementation.Persistence;
using Agile.Planning.Domain.Models;

namespace Agile.Web.Controllers
{
    public class StoriesController : ApiController
    {
        private readonly GetEventStoreRepository _eventStoreRepository;

        public StoriesController(GetEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IHttpActionResult> Post(CreateStoryModel model)
        {
            var id = Guid.NewGuid();
            var story = new Story(id, model.Title, model.Description);
            await _eventStoreRepository.Save(story, Guid.NewGuid(), objects => { });
            return Created(Url.Link("DefaultApi", new {controller = "Stories"}), new {id});
        }
    }

    public class CreateStoryModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}