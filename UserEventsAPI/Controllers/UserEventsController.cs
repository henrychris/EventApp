using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UserEventsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserEventsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }

        [HttpPost]
        public Task<IActionResult> RegisterUserToEvent(EventRegistrationDetails model)
        {
            // receive object in format above and add to DB.

            throw new NotImplementedException();
        }
    }
}
