using EventAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents()
        {
            return Ok(await _unitOfWork.Events.GetAllAsync());
        }

        [HttpGet("GetEventById")]
        public async Task<IActionResult> GetEventById(string id)
        {
            var eventModel = await _unitOfWork.Events.GetByIdAsync(id);

            if (eventModel == null)
                return NotFound(null);

            return Ok(eventModel);
        }

        [HttpGet("GetEventByName")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var eventList = await _unitOfWork.Events.GetEventByNameAsync(name);

            if (eventList.Count <= 0)
            {
                return BadRequest(eventList);
            }
            return Ok(eventList);
        }
    }
}
