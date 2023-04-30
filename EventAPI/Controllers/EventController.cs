using EventAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepo

        public EventController(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo
        }

        [HttpGet("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents()
        {
            return Ok(await _eventRepo.GetAllEventsAsync());
        }

        [HttpGet("GetEventById")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventModel = await _eventRepo.GetEventByIdAsync(id);

            if (eventModel == Event.NotFound)
            {
                // TODO add logging at some point
                return BadRequest(Event.NotFound);
            }
            return Ok(eventModel);
        }

        [HttpGet("GetEventByName")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var eventList = await _eventRepo.GetEventByNameAsync(name);

            if (eventList.Count <= 0)
            {
                return BadRequest(eventList);
            }
            return Ok(eventList);
        }
    }
}
